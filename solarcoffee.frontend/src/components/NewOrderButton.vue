<template>
	<v-dialog v-model="dialog" max-width="1280px" persistent>
		<template v-slot:activator="{ on }">
			<v-btn
				elevation="0"
				color="secondary"
				v-on="on"
				:disabled="disabledButton"
			>
				Add New Order
				<v-icon right>mdi-plus</v-icon>
			</v-btn>
		</template>

		<v-stepper v-model="step">
			<v-stepper-header>
				<v-stepper-step :complete="step > 1" step="1">
					Select customer
				</v-stepper-step>

				<v-divider></v-divider>

				<v-stepper-step :complete="step > 2" step="2">
					Select products
				</v-stepper-step>

				<v-divider></v-divider>

				<v-stepper-step :complete="step > 3" step="3">
					Select payment, delivery, discount
				</v-stepper-step>

				<v-divider></v-divider>

				<v-stepper-step step="4"> Сonfirmation </v-stepper-step>
			</v-stepper-header>

			<v-stepper-items>
				<v-stepper-content step="1">
					<v-card class="mb-12" flat>
						<v-select
							:items="customers"
							label="Customer"
							v-model="order.customerId"
							item-value="id"
							prepend-inner-icon="mdi-alpha-c-box-outline"
							:item-text="
								(item) =>
									`${item.firstName} ${item.lastName}, ${item.phoneNumber}`
							"
						/>
					</v-card>
					<v-btn
						color="primary"
						@click="step++"
						:disabled="order.customerId === -1"
						class="mr-3"
					>
						Continue
					</v-btn>
					<v-btn
						color="primary"
						@click="step--"
						disabled
						class="mr-3"
					>
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="2">
					<v-card class="mb-12" flat>
						<v-select
							prepend-inner-icon="mdi-alpha-t-box-outline"
							:items="products"
							label="Product"
							v-model="product"
							:item-text="
								(item) =>
									`${item.name} [Price: ${item.price}, Arcived: ${item.isArchived}]`
							"
							:item-value="(item) => item"
							@input="productSelected"
						/>

						<v-row v-if="this.orderItem.productId != -1">
							<v-col cols="7">
								<v-text-field
									v-model.number="orderItem.quantity"
									:rules="[
										(v) =>
											(!isNaN(parseInt(v)) && v > 0) ||
											'Quantity has to be more then 0',
									]"
									label="Quantity"
									prepend-inner-icon="mdi-alpha-q-box-outline"
								/>
							</v-col>
							<v-col cols="5">
								<v-row justify="center">
									<v-btn
										color="primary"
										@click="addToOrderItems"
										:disabled="orderItem.quantity <= 0"
										class="ma-5"
									>
										Add to order
									</v-btn>
								</v-row>
							</v-col>
						</v-row>
					</v-card>

					<v-data-table
						v-if="orderItems.length"
						:headers="orderItemHeader"
						:items="orderItems"
						fixed-header
						v-resize="onResize"
						sort-by="product.name"
					>
						<template v-slot:top>
							{{ runningTotal }}
						</template>

						<template #[`item.lineTotal`]="{ item }">
							{{ item.quantity * item.productPrice }}
						</template>
						<template #[`item.actions`]="{ item }">
							<v-row justify="center">
								<v-tooltip top>
									<template v-slot:activator="{ on, attrs }">
										<v-btn
											v-on="on"
											v-bind="attrs"
											@click="removeFromOrderItems(item)"
											elevation="0"
											icon
										>
											<v-icon color="red"
												>mdi-cart-remove</v-icon
											>
										</v-btn>
									</template>
									<span>Remove</span>
								</v-tooltip>
							</v-row>
						</template>
					</v-data-table>

					<v-btn
						color="primary"
						@click="step++"
						:disabled="!orderItems.length"
						class="mr-3"
					>
						Continue
					</v-btn>
					<v-btn color="primary" @click="step--" class="mr-3">
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="3">
					<v-card class="mb-12" flat></v-card>

					<v-btn color="primary" @click="step++" class="mr-3">
						Continue
					</v-btn>
					<v-btn color="primary" @click="step--" class="mr-3">
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="4">
					<v-card class="mb-12" flat></v-card>

					<v-btn
						color="primary"
						@click="step++"
						disabled
						class="mr-3"
					>
						Continue
					</v-btn>
					<v-btn color="primary" @click="step--" class="mr-3">
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>
			</v-stepper-items>
		</v-stepper>
		<!-- <v-card class="pa-5" v-if="dialog">
			<v-card-title class="mb-5">
				<v-row align="center" justify="space-between">
					Creating new order
					<v-btn
						:disabled="!formValid"
						elevation="0"
						color="secondary"
						@click="addNewOrder"
					>
						<v-icon color="white">mdi-plus</v-icon>
					</v-btn>
				</v-row>
			</v-card-title>
			<v-card-text>
				<v-form v-model="formValid">
					<v-row>
						<v-select
							:items="customers"
							label="Customer"
							v-model="order.customerId"
							item-value="id"
							:item-text="
								(item) =>
									`${item.firstName} ${item.lastName}, ${item.phoneNumber}`
							"
						/>

						<v-select
							:items="payments"
							label="Payment"
							v-model="order.paymentId"
							item-value="id"
							:item-text="
								(item) => `${item.name} [${item.description}]`
							"
						/>

						<v-select
							:items="deliveries"
							label="Delivery"
							v-model="order.deliveryId"
							item-value="id"
							:item-text="
								(item) => `${item.name} [${item.description}]`
							"
						/>

						<v-select
							:items="discounts"
							label="Discount"
							v-model="order.discountId"
							item-value="id"
							:item-text="
								(item) => `${item.name} [${item.description}]`
							"
						/>

						<v-select
							:items="products"
							label="Product"
							v-model="order.paymentId"
							item-value="id"
							:item-text="
								(item) => `${item.name} [${item.price}]`
							"
						/>
					</v-row>
				</v-form>
			</v-card-text>
		</v-card> -->
	</v-dialog>
</template>

<script lang="ts">
import CustoemrService from "@/services/customer-service";
import OrderService from "@/services/order-service";
import ProductService from "@/services/product-service";
import OrderSettingsService from "@/services/order-settings-service";
import { Component, Vue, Prop, Inject, Watch } from "vue-property-decorator";
import ICustomer from "@/models/customer";
import IPayment from "@/models/payment";
import IDelivery from "@/models/delivery";
import IDiscount from "@/models/discount";
import IProduct from "@/models/product";
import { INewOrder, OrderStatus } from "@/models/order";
import IOrderItem from "@/models/order-item";
import { DataTableHeader } from "vuetify";

@Component
export default class NewOrderButton extends Vue {
	@Inject() readonly orderService!: OrderService;
	@Inject() readonly customerService!: CustoemrService;
	@Inject() readonly productService!: ProductService;
	@Inject() readonly orderSettingsService!: OrderSettingsService;

	@Prop({ required: true })
	disabledButton!: boolean;

	step = 1;

	order: INewOrder = {
		id: 0,
		customerId: -1,
		items: [],
		orderStatus: OrderStatus.Created,
		paymentId: -1,
		deliveryId: -1,
		discountId: -1,
		additionalInfo: "",
		totalPrice: 0,
	};

	dialog = false;
	formValid = false;

	customers: ICustomer[] = [];
	payments: IPayment[] = [];
	deliveries: IDelivery[] = [];
	discounts: IDiscount[] = [];

	products: IProduct[] = [];

	product: IProduct | null = null;

	orderItems: IOrderItem[] = [];
	orderItem: IOrderItem = {
		id: 0,
		productName: "",
		productPrice: 0,
		productId: -1,
		quantity: 0,
	};

	get orderItemHeader(): DataTableHeader[] {
		return [
			{ text: "Product", value: "productName", align: "center" },
			{ text: "Quantity", value: "quantity", align: "center" },
			{ text: "Price", value: "productPrice", align: "center" },
			{
				text: "Line total",
				value: "lineTotal",
				align: "center",
				sortable: false,
			},
			{
				text: "Actions",
				value: "actions",
				align: "center",
				sortable: false,
			},
		];
	}

	get runningTotal(): number {
		return this.orderItems.reduce(
			(a, b) => a + b["productPrice"] * b["quantity"],
			0
		);
	}

	@Watch("dialog")
	async onDialogChange(newState: boolean): Promise<void> {
		if (newState) {
			await this.getCustomers();
		}
	}

	@Watch("step")
	async onStepChange(newStep: number): Promise<void> {
		if (newStep === 2) {
			await this.getAvailableProducts();
		}
		if (newStep === 3) {
			await this.getOrderSettings();
		}
	}

	async getCustomers(): Promise<void> {
		this.customers = await this.customerService.getCustomers();
	}

	async getOrderSettings(): Promise<void> {
		const orderSettings =
			await this.orderSettingsService.getOrderSettings();

		this.payments = orderSettings.payments;
		this.deliveries = orderSettings.deliveries;
		this.discounts = orderSettings.discounts;
	}

	async getAvailableProducts(): Promise<void> {
		this.products = await this.productService.getProducts();
	}

	addToOrderItems(): void {
		if (this.orderItem.quantity > 0) {
			const index = this.orderItems.findIndex(
				(oi) => oi.productId === this.orderItem.productId
			);
			if (index > -1) {
				this.orderItems[index].quantity += +this.orderItem.quantity;
			} else {
				if (this.orderItems.length) {
					this.orderItem.id =
						this.orderItems[this.orderItems.length - 1].id + 1;
				}
				this.orderItems.push(this.orderItem);
			}
		}

		this.product = null;

		this.orderItem = {
			id: 0,
			productName: "",
			productPrice: 0,
			productId: -1,
			quantity: 0,
		};
	}

	productSelected(product: IProduct): void {
		this.orderItem.productId = product.id;
		this.orderItem.productName = product.name;
		this.orderItem.productPrice = product.price;
	}

	addNewOrder(): void {
		//
	}

	removeFromOrderItems(item: IOrderItem): void {
		const index = this.orderItems.indexOf(item);
		if (index > -1) {
			this.orderItems.splice(index, 1);
		}
	}

	cancel(): void {
		this.dialog = false;
		this.product = null;

		this.order = {
			id: 0,
			customerId: -1,
			items: [],
			orderStatus: OrderStatus.Created,
			paymentId: -1,
			deliveryId: -1,
			discountId: -1,
			additionalInfo: "",
			totalPrice: 0,
		};

		this.orderItem = {
			id: 0,
			productName: "",
			productPrice: 0,
			productId: -1,
			quantity: 0,
		};

		this.orderItems = [];
	}

	onResize(): void {
		const wrapper = document.getElementsByClassName(
			"v-data-table__wrapper"
		)[0] as HTMLDivElement;

		if (wrapper) {
			wrapper.style.maxHeight =
				window.innerWidth > 1888 ? "70vh" : "62vh";
			wrapper.style.height = "100%";
		}
	}
}
</script>

<style scoped></style>
