<template>
	<v-dialog v-model="dialog" max-width="1280px" persistent>
		<template v-slot:activator="{ on }">
			<v-btn
				color="secondary"
				v-on="on"
				:disabled="disabledButton"
				class="ml-2"
			>
				<v-icon>mdi-cog-outline</v-icon>
			</v-btn>
		</template>
		<v-card class="pa-5" v-if="dialog">
			<v-toolbar flat class="pa-0">
				<v-layout align-center justify-start class="text-h6">
					Order Settings
				</v-layout>
				<v-layout align-center justify-end>
					<v-toolbar-items>
						<v-btn color="secondary" @click="dialog = !dialog">
							<v-icon color="white">mdi-close</v-icon>
						</v-btn>
					</v-toolbar-items>
				</v-layout>
			</v-toolbar>
			<v-divider />
			<v-tabs vertical v-model="selectedTabMain">
				<v-tab>
					<v-icon left> mdi-credit-card-outline </v-icon>
					Payment
				</v-tab>
				<v-tab>
					<v-icon left> mdi-truck-delivery-outline </v-icon>
					Delivery
				</v-tab>
				<v-tab>
					<v-icon left> mdi-sale </v-icon>
					Discount
				</v-tab>

				<v-tabs-items v-model="selectedTabMain">
					<v-tab-item>
						<v-tabs v-model="selectedTabPayment">
							<v-tab v-for="n in 2" :key="n">
								{{ ["", "Add New Method", "Available"][n] }}
							</v-tab>
						</v-tabs>

						<v-tabs-items v-model="selectedTabPayment">
							<v-tab-item>
								<v-card flat>
									<v-card-text>
										<v-form
											v-model="formPaymentValid"
											ref="paymentForm"
										>
											<v-text-field
												v-model="payment.name"
												:rules="[
													(v) =>
														!!v ||
														'Field is required',
												]"
												label="Name"
												clearable
												maxlength="100"
												counter
												prepend-inner-icon="mdi-format-title"
											/>

											<v-textarea
												v-model="payment.description"
												label="Description"
												clearable
												counter
												maxlength="1000"
												prepend-inner-icon="mdi-card-text-outline"
											/>
										</v-form>
									</v-card-text>
									<v-card-actions>
										<v-spacer />
										<v-btn
											color="blue darken-1"
											text
											:disabled="!formPaymentValid"
											@click="onSavePayment"
										>
											Add new payment method
										</v-btn>
									</v-card-actions>
								</v-card>
							</v-tab-item>
							<v-tab-item
								><v-data-table
									:headers="paymentHeader"
									:items="payments"
									:loading="!dataLoaded"
									fixed-header
								/>
							</v-tab-item>
						</v-tabs-items>
					</v-tab-item>
					<v-tab-item>
						<v-tabs v-model="selectedTabDelivery">
							<v-tab v-for="n in 2" :key="n">
								{{ ["", "Add New Method", "Available"][n] }}
							</v-tab>
						</v-tabs>

						<v-tabs-items v-model="selectedTabDelivery">
							<v-tab-item>
								<v-card flat>
									<v-card-text>
										<v-form
											v-model="formDeliveryValid"
											ref="deliveryForm"
										>
											<v-text-field
												v-model="delivery.name"
												:rules="[
													(v) =>
														!!v ||
														'Field is required',
												]"
												label="Name"
												clearable
												maxlength="100"
												counter
												prepend-inner-icon="mdi-format-title"
											/>

											<v-textarea
												v-model="delivery.description"
												label="Description"
												clearable
												counter
												maxlength="1000"
												prepend-inner-icon="mdi-card-text-outline"
											/>

											<v-text-field
												v-model.number="delivery.price"
												:rules="[
													(v) =>
														(!isNaN(parseInt(v)) &&
															v >= 0) ||
														'Price has to be positive number',
												]"
												label="Price"
												type="number"
												prepend-inner-icon="mdi-cash-multiple"
											/>
										</v-form>
									</v-card-text>
									<v-card-actions>
										<v-spacer />
										<v-btn
											color="blue darken-1"
											text
											:disabled="!formDeliveryValid"
											@click="onSaveDelivery"
										>
											Add new delivery method
										</v-btn>
									</v-card-actions>
								</v-card>
							</v-tab-item>
							<v-tab-item>
								<v-data-table
									:headers="deliveryHeader"
									:items="deliveries"
									:loading="!dataLoaded"
									fixed-header
								></v-data-table>
							</v-tab-item>
						</v-tabs-items>
					</v-tab-item>

					<v-tab-item>
						<v-tabs v-model="selectedTabDiscount">
							<v-tab v-for="n in 2" :key="n">
								{{ ["", "Add New", "Available"][n] }}
							</v-tab>
						</v-tabs>

						<v-tabs-items v-model="selectedTabDiscount">
							<v-tab-item>
								<v-card flat>
									<v-card-text>
										<v-form
											v-model="formDiscountValid"
											ref="discountForm"
										>
											<v-text-field
												v-model="discount.name"
												:rules="[
													(v) =>
														!!v ||
														'Field is required',
												]"
												label="Name"
												clearable
												maxlength="100"
												counter
												prepend-inner-icon="mdi-format-title"
											/>

											<v-textarea
												v-model="discount.description"
												label="Description"
												clearable
												counter
												maxlength="1000"
												prepend-inner-icon="mdi-card-text-outline"
											/>

											<v-text-field
												v-model.number="
													discount.discountPercent
												"
												:rules="[
													(v) =>
														(!isNaN(parseInt(v)) &&
															v >= 0) ||
														'Price has to be positive number',
												]"
												label="Discount percent"
												type="number"
												prepend-inner-icon="mdi-sale"
											/>
											<v-card-actions>
												<v-spacer />
												<v-btn
													color="blue darken-1"
													text
													:disabled="
														!formDiscountValid
													"
													@click="onSaveDiscount"
												>
													Add new discount
												</v-btn>
											</v-card-actions>
										</v-form>
									</v-card-text>
								</v-card>
							</v-tab-item>
							<v-tab-item>
								<v-data-table
									:headers="discountHeader"
									:items="discounts"
									:loading="!dataLoaded"
									fixed-header
								>
								</v-data-table>
							</v-tab-item>
						</v-tabs-items>
					</v-tab-item>
				</v-tabs-items>
			</v-tabs>
		</v-card>
		<v-snackbar
			v-model="snackbar"
			:timeout="timeout"
			absolute
			centered
			color="green"
		>
			Added
		</v-snackbar>
	</v-dialog>
</template>

<script lang="ts">
import IDelivery from "@/models/request/order/delivery";
import IDiscount from "@/models/request/order/discount";
import IPayment from "@/models/request/order/payment";
import OrderSettingsService from "@/services/order-settings-service";
import { Component, Vue, Prop, Inject, Watch } from "vue-property-decorator";
import { DataTableHeader } from "vuetify";

@Component
export default class OrderSettings extends Vue {
	@Inject() readonly orderSettingsService!: OrderSettingsService;

	@Prop({ required: true })
	disabledButton!: boolean;

	selectedTabMain = 0;
	selectedTabDelivery = 0;
	selectedTabDiscount = 0;
	selectedTabPayment = 0;

	payment: IPayment = {
		id: 0,
		name: "",
		description: "",
	};

	payments: IPayment[] = [];

	delivery: IDelivery = {
		id: 0,
		name: "",
		description: "",
		price: 0,
	};

	deliveries: IDelivery[] = [];

	discount: IDiscount = {
		id: 0,
		name: "",
		description: "",
		discountPercent: 0,
	};

	discounts: IDiscount[] = [];

	dialog = false;
	formPaymentValid = false;
	formDeliveryValid = false;
	formDiscountValid = false;
	snackbar = false;
	timeout = 2000;
	dataLoaded = false;

	@Watch("dialog")
	async onDialogChange(newState: boolean): Promise<void> {
		if (newState) {
			await this.initialize();
		}
	}

	get discountHeader(): DataTableHeader[] {
		return [
			{ text: "Name", value: "name", align: "center" },
			{ text: "Description", value: "description", align: "center" },
			{
				text: "Discount percent",
				value: "discountPercent",
				align: "center",
			},
		];
	}

	get deliveryHeader(): DataTableHeader[] {
		return [
			{ text: "Name", value: "name", align: "center" },
			{ text: "Description", value: "description", align: "center" },
			{ text: "Price", value: "price", align: "center" },
		];
	}

	get paymentHeader(): DataTableHeader[] {
		return [
			{ text: "Name", value: "name", align: "center" },
			{ text: "Description", value: "description", align: "center" },
		];
	}

	async initialize(): Promise<void> {
		this.dataLoaded = false;
		const orderSettings =
			await this.orderSettingsService.getOrderSettings();

		this.payments = orderSettings.payments;
		this.deliveries = orderSettings.deliveries;
		this.discounts = orderSettings.discounts;

		this.dataLoaded = true;
	}

	async onSavePayment(): Promise<void> {
		await this.orderSettingsService.addNewPaymentMethod(this.payment);
		this.payments.push(this.payment);
		this.payment = {
			id: 0,
			name: "",
			description: "",
		};
		this.snackbar = true;
		const form = this.$refs.paymentForm as Vue & {
			resetValidation: () => void;
		};
		if (form) {
			form.resetValidation();
		}
		//
	}

	async onSaveDelivery(): Promise<void> {
		await this.orderSettingsService.addNewdeliveryMethod(this.delivery);
		this.deliveries.push(this.delivery);
		this.delivery = {
			id: 0,
			name: "",
			price: 0,
			description: "",
		};
		this.snackbar = true;

		const form = this.$refs.deliveryForm as Vue & {
			resetValidation: () => void;
		};
		if (form) {
			form.resetValidation();
		}
	}

	async onSaveDiscount(): Promise<void> {
		await this.orderSettingsService.addNewDiscount(this.discount);
		this.discounts.push(this.discount);
		this.discount = {
			id: 0,
			name: "",
			discountPercent: 0,
			description: "",
		};
		this.snackbar = true;

		const form = this.$refs.discountForm as Vue & {
			resetValidation: () => void;
		};
		if (form) {
			form.resetValidation();
		}
	}
}
</script>

<style scoped>
::v-deep .v-snack__content {
	text-align: center;
}
</style>
