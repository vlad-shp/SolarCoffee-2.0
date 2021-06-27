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
		<loading msg="Connecting to server..." v-if="!dataLoaded" />
		<v-stepper v-model="step" v-else>
			<v-stepper-header>
				<v-stepper-step
					:complete="step > 1"
					step="1"
					class="solar-button"
				>
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

				<v-divider />

				<v-stepper-step step="4"> Сonfirmation </v-stepper-step>
			</v-stepper-header>
			<v-stepper-items>
				<v-stepper-content step="1">
					<!-- <v-stepper-content step="1"> -->
					<v-card class="mb-12" flat>
						<v-select
							:items="customerSelect"
							label="Customer"
							v-model="newOrder.customer"
							prepend-inner-icon="mdi-alpha-c-box-outline"
						/>
					</v-card>
					<v-btn
						color="primary"
						@click="step++"
						:disabled="newOrder.customer.id === -1"
						class="mr-3 solar-button"
					>
						Continue
					</v-btn>
					<v-btn
						color="primary"
						@click="step--"
						disabled
						class="mr-3 solar-button"
					>
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="2">
					<v-card class="mb-12" flat>
						<v-select
							prepend-inner-icon="mdi-alpha-t-box-outline"
							:items="productSelect"
							label="Product"
							v-model="orderItem.product"
							@change="productSelected"
						/>
						<v-row>
							<v-col cols="7">
								<v-text-field
									:disabled="
										orderItem.product.id == -1 ||
										this.maxQuantityOnHand === 0
									"
									v-model.number="orderItem.quantity"
									:rules="[
										(v) =>
											(!isNaN(parseInt(v)) &&
												v > 0 &&
												v <= maxQuantityOnHand) ||
											errorMsg,
									]"
									label="Quantity"
									prepend-inner-icon="mdi-alpha-q-box-outline"
								/>
							</v-col>
							<v-col cols="2">
								<v-text-field
									readonly
									label="Awailable"
									:value="maxQuantityOnHand"
									prepend-inner-icon="mdi-alpha-a-box-outline"
								/>
							</v-col>
							<v-col cols="3">
								<v-row justify="center">
									<v-btn
										color="primary"
										@click="addToOrderItems"
										:disabled="
											orderItem.quantity <= 0 ||
											orderItem.quantity >
												maxQuantityOnHand
										"
										class="ma-5 solar-button"
									>
										Add to order
									</v-btn>
								</v-row>
							</v-col>
						</v-row>
					</v-card>

					<v-slide-y-transition>
						<v-data-table
							v-if="orderItems.length"
							:headers="orderItemHeader"
							:items="orderItems"
							fixed-header
							v-resize="onResize"
							sort-by="product.name"
						>
							<template v-slot:[`item.product.price`]="{ item }">
								{{ item.product.price | price }}
							</template>

							<template slot="body.append">
								<tr class="font-weight-bold">
									<td></td>
									<td></td>
									<td></td>
									<td class="text-center">
										{{ runningTotal | price }}
									</td>
									<td></td>
								</tr>
							</template>

							<template #[`item.lineTotal`]="{ item }">
								{{
									(item.quantity * item.product.price) | price
								}}
							</template>
							<template #[`item.actions`]="{ item }">
								<v-row justify="center">
									<v-tooltip top>
										<template
											v-slot:activator="{ on, attrs }"
										>
											<v-btn
												v-on="on"
												v-bind="attrs"
												@click="
													removeFromOrderItems(item)
												"
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
					</v-slide-y-transition>
					<v-btn
						color="primary"
						@click="step++"
						:disabled="!orderItems.length"
						class="mr-3 solar-button"
					>
						Continue
					</v-btn>
					<v-btn
						color="primary solar-button"
						@click="step--"
						class="mr-3"
					>
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="3">
					<v-card class="mb-12" flat>
						<v-select
							:items="paymentSelect"
							label="Payment"
							v-model="newOrder.payment"
							prepend-inner-icon="mdi-alpha-p-box-outline"
						/>
						<v-select
							:items="deliverySelect"
							label="Delivery"
							v-model="newOrder.delivery"
							prepend-inner-icon="mdi-alpha-p-box-outline"
						/>
						<v-select
							:items="discountSelect"
							label="Discount"
							v-model="newOrder.discount"
							prepend-inner-icon="mdi-alpha-d-box-outline"
						/>
					</v-card>

					<v-btn
						color="primary solar-button"
						@click="step++"
						class="mr-3"
					>
						Continue
					</v-btn>
					<v-btn
						color="primary solar-button"
						@click="step--"
						class="mr-3"
					>
						Back
					</v-btn>

					<v-btn text @click="cancel"> Cancel </v-btn>
				</v-stepper-content>

				<v-stepper-content step="4">
					<v-card
						class="mb-12"
						flat
						style="
							overflow-y: scroll;
							overflow-x: clip;
							height: 400px;
						"
					>
						<v-row>
							<v-col cols="9">
								<v-img
									id="imgLogo"
									alt="Solar coffee logo"
									src="../assets/logo_v2.png"
									max-height="100"
									max-width="250"
									aspect-ratio="1.7"
									contain
								/>
							</v-col>
							<v-col cols="3" class="text-right">
								Invoice #: {{ newOrderId }}<br />
								Created: {{ new Date() | humanizeDate }}<br />
								Due: {{ new Date() | humanizeDate }}
							</v-col>
						</v-row>
						<v-row class="mb-4">
							<v-col cols="9">
								SolarCoffee 2.0, Inc.<br />
								12345 Sunny Road<br />
								Sunnyville, CA 12345
							</v-col>
							<v-col cols="3" class="text-right">
								{{
									`${newOrder.customer.firstName}	${newOrder.customer.firstName}`
								}}<br />
								{{ newOrder.customer.phoneNumber }}<br />
								{{ newOrder.customer.email }}
							</v-col>
						</v-row>
						<v-divider />
						<v-data-table
							:headers="orderItemCompleteHeader"
							:items="orderItems"
							sort-by="product.name"
							hide-default-footer
							id="my-table"
						>
							<template v-slot:[`item.product.price`]="{ item }">
								{{ item.product.price | price }}
							</template>

							<template #[`item.lineTotal`]="{ item }">
								{{
									(item.quantity * item.product.price) | price
								}}
							</template>
							<template #footer>
								<v-divider />
								<v-row>
									<v-col cols="9"> </v-col>
									<v-col cols="3">
										<div
											class="
												font-weight-bold
												text-right
												px-3
											"
										>
											Total: {{ runningTotal | price }}
										</div>
									</v-col>
								</v-row>
							</template>
						</v-data-table>

						<v-row class="pt-5 px-3">
							<v-col cols="9">
								<span class="font-weight-bold"
									>Delivery Address</span
								><br />
								Postal Code<br />
								City<br />
								Country
							</v-col>
							<v-col cols="3" class="text-right">
								<span class="font-weight-bold">{{
									newOrder.customer.primaryAddress
										.addressLine1
								}}</span>
								<br />
								{{
									newOrder.customer.primaryAddress.postalCode
								}}
								<br />
								{{ newOrder.customer.primaryAddress.city }}
								<br />

								{{ newOrder.customer.primaryAddress.country }}
							</v-col>
						</v-row>
						<v-divider />

						<v-row class="pt-5 px-3">
							<v-col cols="9">
								<span class="font-weight-bold"
									>Delivery Method</span
								><br />
								Price
							</v-col>
							<v-col cols="3" class="text-right">
								<span class="font-weight-bold">{{
									newOrder.delivery.name
								}}</span
								><br />
								{{ newOrder.delivery.price | price }}
							</v-col>
						</v-row>
						<v-divider />
						<v-row class="pt-4 px-3">
							<v-col cols="9">
								<span class="font-weight-bold">Discount</span
								><br />
								Percent
							</v-col>
							<v-col cols="3" class="text-right">
								<span class="font-weight-bold">{{
									newOrder.discount.name
								}}</span
								><br />
								{{ newOrder.discount.discountPercent }}%
							</v-col>
						</v-row>
						<v-divider />
						<v-row class="pt-4 px-3">
							<v-col cols="8"> </v-col>
							<v-col cols="4" class="text-right">
								<v-row>
									<v-col
										cols="8"
										class="font-weight-bold text-right"
									>
										Total price:
									</v-col>
									<v-col cols="4">
										{{ runningTotal | price }}<br />
										+ {{ newOrder.delivery.price | price
										}}<br />
										-
										{{
											newOrder.discount.discountPercent
										}}%<br />
										<v-divider />
										= {{ orderTotalPrice | price }}
									</v-col>
								</v-row>
							</v-col>
						</v-row>

						<v-row class="pt-4 px-3">
							<v-col cols="9" class="font-weight-bold">
								Payment Method
							</v-col>
							<v-col cols="3" class="font-weight-bold text-right">
								VISA
							</v-col>
						</v-row>
						<v-divider />
					</v-card>

					<v-btn
						color="primary"
						@click="confirmOrder"
						class="mr-3 solar-button"
						v-if="!showSuccessfulMsg"
					>
						Confirm
					</v-btn>
					<v-btn
						color="primary"
						@click="step--"
						v-if="!showSuccessfulMsg"
						class="mr-3 solar-button"
					>
						Back
					</v-btn>

					<v-alert dense text type="success" v-if="showSuccessfulMsg">
						Order was created successfully!
					</v-alert>

					<v-btn text @click="cancel" v-if="!showSuccessfulMsg">
						Cancel
					</v-btn>
					<v-btn
						text
						@click="close"
						class="ma-3 solar-button"
						color="primary"
						v-else
					>
						Close
					</v-btn>
				</v-stepper-content>
			</v-stepper-items>
		</v-stepper>
		<v-dialog v-model="showGeneratedPdf" content-class="dialog-wrapper">
			<iframe
				id="ifrm"
				:src="generatedPdfUri"
				style="height: 90vh"
				width="100%"
			></iframe>
		</v-dialog>
	</v-dialog>
</template>

<script lang="ts">
import CustoemrService from "@/services/customer-service";
import OrderService from "@/services/order-service";
import ProductService from "@/services/product-service";
import OrderSettingsService from "@/services/order-settings-service";
import { Component, Vue, Prop, Inject, Watch } from "vue-property-decorator";
import ICustomer from "@/models/request/customer/customer";
import IPayment from "@/models/request/order/payment";
import IDelivery from "@/models/request/order/delivery";
import IDiscount from "@/models/request/order/discount";
import IProduct from "@/models/request/product";
import { INewOrder, OrderStatus } from "@/models/request/order/new-order";
import { DataTableHeader } from "vuetify";
import IOrderItemView from "@/models/view/order-item-view";
import IOrderView from "@/models/view/order-view";
import SelectItem from "@/models/select-item";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import moment from "moment";
import Loading from "@/components/Loading.vue";
import IOrderItem from "@/models/request/order/order-item";
import InventoryService from "@/services/inventory-service";

@Component({ components: { Loading } })
export default class NewOrderButton extends Vue {
	@Inject() readonly orderService!: OrderService;
	@Inject() readonly customerService!: CustoemrService;
	@Inject() readonly productService!: ProductService;
	@Inject() readonly orderSettingsService!: OrderSettingsService;
	@Inject() readonly inventoryService!: InventoryService;

	@Prop({ required: true })
	disabledButton!: boolean;

	@Prop({ required: true })
	newOrderId!: number;

	step = 1;

	showGeneratedPdf = false;
	showSuccessfulMsg = false;

	generatedPdfUri = "";

	newOrder: IOrderView = {
		id: -1,
		customer: {
			id: -1,
			primaryAddress: {
				id: -1,
				createdOn: new Date(),
				updatedOn: new Date(),
				addressLine1: "",
				addressLine2: "",
				city: "",
				state: "",
				postalCode: "",
				country: "",
			},
			firstName: "",
			lastName: "",
			email: "",
			phoneNumber: "",
			createdOn: new Date(),
			updatedOn: new Date(),
		},
		delivery: {
			id: -1,
			name: "",
			description: "",
			price: 0,
		},
		payment: {
			id: -1,
			name: "",
			description: "",
		},
		discount: {
			id: -1,
			name: "",
			description: "",
			discountPercent: 0,
		},
		orderItems: [],
		orderStatus: OrderStatus.Created,
		additionalInfo: "",
		totalPrice: 0,
		createdOn: new Date(),
		updatedOn: new Date(),
	};

	dialog = false;
	formValid = false;

	customers: ICustomer[] = [];
	payments: IPayment[] = [];
	deliveries: IDelivery[] = [];
	discounts: IDiscount[] = [];

	products: IProduct[] = [];

	orderItems: IOrderItemView[] = [];
	orderItem: IOrderItemView = {
		id: 0,
		product: {
			id: -1,
			name: "",
			description: "",
			price: 0,
			isTaxable: false,
			isArchived: false,
			createdOn: new Date(),
			updatedOn: new Date(),
		},
		quantity: 0,
	};

	get orderItemHeader(): DataTableHeader[] {
		return [
			{
				text: "Product",
				value: "product.name",
				align: "center",
				divider: true,
			},
			{
				text: "Quantity",
				value: "quantity",
				align: "center",
				divider: true,
			},
			{
				text: "Unit Cost",
				value: "product.price",
				align: "center",
				divider: true,
			},
			{
				text: "Price",
				value: "lineTotal",
				align: "center",
				divider: true,
			},
			{
				text: "Actions",
				value: "actions",
				align: "center",
				sortable: false,
			},
		];
	}

	get orderItemCompleteHeader(): DataTableHeader[] {
		return [
			{
				text: "Product",
				value: "product.name",
				align: "center",
				sortable: false,
				divider: true,
			},
			{
				text: "Quantity",
				value: "quantity",
				align: "center",
				sortable: false,
				divider: true,
			},
			{
				text: "Unit Cost",
				value: "product.price",
				align: "center",
				sortable: false,
				divider: true,
			},
			{
				text: "Price",
				value: "lineTotal",
				align: "end",
				sortable: false,
			},
		];
	}

	get orderTotalPrice(): number {
		const total = this.runningTotal + this.newOrder.delivery.price;
		this.newOrder.totalPrice =
			total - total * 0.01 * this.newOrder.discount.discountPercent;
		return this.newOrder.totalPrice;
	}

	get runningTotal(): number {
		return this.orderItems.reduce(
			(a, b) => a + b["product"]["price"] * b["quantity"],
			0
		);
	}

	get customerSelect(): SelectItem<ICustomer>[] {
		return this.customers.map(
			(c) =>
				new SelectItem(
					[c.firstName, c.lastName + ",", c.phoneNumber].join(" "),
					c
				)
		);
	}

	get productSelect(): SelectItem<IProduct>[] {
		return this.products.map(
			(c) =>
				new SelectItem([c.name + ",", "price:", c.price].join(" "), c)
		);
	}

	get discountSelect(): SelectItem<IDiscount>[] {
		return this.discounts.map(
			(c) =>
				new SelectItem(
					[c.name + ",", "discount percent:", c.discountPercent].join(
						" "
					),
					c
				)
		);
	}

	get deliverySelect(): SelectItem<IDelivery>[] {
		return this.deliveries.map(
			(c) =>
				new SelectItem([c.name + ",", "price:", c.price].join(" "), c)
		);
	}

	get paymentSelect(): SelectItem<IPayment>[] {
		return this.payments.map((c) => new SelectItem(c.name, c));
	}

	get errorMsg(): string {
		return `Quantity has to be more then 0 and less than ${this.maxQuantityOnHand}`;
	}

	dataLoaded = false;

	@Watch("dialog")
	async onDialogChange(newState: boolean): Promise<void> {
		if (newState) {
			this.dataLoaded = false;
			await this.getCustomers();
			this.dataLoaded = true;
		}
	}

	@Watch("step")
	async onStepChange(newStep: number): Promise<void> {
		if (newStep === 2) {
			await this.getAvailableProducts();
			this.showSuccessfulMsg = false;
		}
		if (newStep === 3) {
			await this.getOrderSettings();

			if (
				this.payments.length &&
				this.discounts.length &&
				this.deliveries.length
			) {
				if (this.newOrder.payment.id === -1) {
					this.newOrder.payment = this.payments[0];
					this.newOrder.discount = this.discounts[0];
					this.newOrder.delivery = this.deliveries[0];
				}
			}
		}
	}

	async quantityOnHand(productId: number): Promise<number> {
		return (
			await this.inventoryService.getInventoryItemByProductId(productId)
		).quantityOnHand;
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
		if (
			this.orderItem.quantity > 0 &&
			this.orderItem.quantity <= this.maxQuantityOnHand
		) {
			const index = this.orderItems.findIndex(
				(oi) => oi.product.id === this.orderItem.product.id
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

		this.orderItem = {
			id: -1,
			product: {
				id: -1,
				name: "",
				description: "",
				price: 0,
				isTaxable: false,
				isArchived: false,
				createdOn: new Date(),
				updatedOn: new Date(),
			},
			quantity: 0,
		};

		this.maxQuantityOnHand = 0;
	}

	maxQuantityOnHand = 0;

	async productSelected(product: IProduct): Promise<void> {
		this.maxQuantityOnHand = await this.quantityOnHand(product.id);

		const itemInOrderItems = this.orderItems.find(
			(item) => item.product.id === product.id
		);

		if (itemInOrderItems) {
			this.maxQuantityOnHand -= itemInOrderItems.quantity;
		}

		if (this.maxQuantityOnHand < 0) {
			this.maxQuantityOnHand = 0;
		}
	}

	discountSelected(discount: IDiscount): void {
		this.newOrder.discount = discount;
	}

	paymentSelected(payment: IPayment): void {
		this.newOrder.payment = payment;
	}

	deliverySelected(delivery: IDelivery): void {
		this.newOrder.delivery = delivery;
	}

	removeFromOrderItems(item: IOrderItemView): void {
		const index = this.orderItems.indexOf(item);
		if (index > -1) {
			this.orderItems.splice(index, 1);
		}
	}

	close(): void {
		this.cancel();
		this.$emit("addedNewOrder");
	}

	cancel(): void {
		this.dialog = false;
		this.step = 1;

		this.newOrder = {
			id: -1,
			customer: {
				id: -1,
				primaryAddress: {
					id: -1,
					createdOn: new Date(),
					updatedOn: new Date(),
					addressLine1: "",
					addressLine2: "",
					city: "",
					state: "",
					postalCode: "",
					country: "",
				},
				firstName: "",
				lastName: "",
				email: "",
				phoneNumber: "",
				createdOn: new Date(),
				updatedOn: new Date(),
			},
			delivery: {
				id: -1,
				name: "",
				description: "",
				price: 0,
			},
			payment: {
				id: -1,
				name: "",
				description: "",
			},
			discount: {
				id: -1,
				name: "",
				description: "",
				discountPercent: 0,
			},
			orderItems: [],
			orderStatus: OrderStatus.Created,
			additionalInfo: "",
			totalPrice: 0,
			createdOn: new Date(),
			updatedOn: new Date(),
		};

		this.orderItem = {
			id: 0,
			product: {
				id: -1,
				name: "",
				description: "",
				price: 0,
				isTaxable: false,
				isArchived: false,
				createdOn: new Date(),
				updatedOn: new Date(),
			},
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
			wrapper.style.overflowY = "auto";
		}
	}

	async confirmOrder(): Promise<void> {
		this.newOrder.orderItems = this.orderItems;

		const itemsMap: IOrderItem[] = [];

		for (const index in this.orderItems) {
			itemsMap.push({
				id: this.orderItems[index].id,
				productId: this.orderItems[index].product.id,
				quantity: this.orderItems[index].quantity,
			});
		}

		const newOrder: INewOrder = {
			id: this.newOrder.id,
			orderItems: itemsMap,
			customerId: this.newOrder.customer.id,
			orderStatus: this.newOrder.orderStatus,
			deliveryId: this.newOrder.delivery.id,
			paymentId: this.newOrder.payment.id,
			discountId: this.newOrder.discount.id,
			additionalInfo: this.newOrder.additionalInfo,
			totalPrice: this.newOrder.totalPrice,
		};

		await this.orderService.addNewOrder(newOrder);

		this.showSuccessfulMsg = true;

		this.generatePDF();
	}

	generatePDF(): void {
		//
		const pdf = new jsPDF("p", "cm", "a4");
		const imgData =
			"data:image/png;base64,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";

		//logo-block
		pdf.addImage(imgData, "PNG", 1, 1, 7, 0.8);
		//end-logo-block

		pdf.setFontSize(10);

		//order-info-block
		pdf.text(`Invoice #: ${this.newOrderId}`, 20, 0.7, { align: "right" });
		pdf.text(
			`Created: ${moment(new Date()).format("MMMM Do YYYY")}`,
			20,
			1.5,
			{ align: "right" }
		);
		pdf.text(`Due: ${moment(new Date()).format("MMMM Do YYYY")}`, 20, 2.3, {
			align: "right",
		});
		//end-order-info-block

		//company-info-block
		pdf.text("SolarCoffee 2.0, Inc.", 1, 4, { align: "left" });
		pdf.text("12345 Sunny Road", 1, 4.8, { align: "left" });
		pdf.text("Sunnyville, CA 12345", 1, 5.6, {
			align: "left",
		});
		//end-company-info-block

		//customer-info-block
		pdf.text(
			`${this.newOrder.customer.firstName} ${this.newOrder.customer.lastName}`,
			20,
			4,
			{ align: "right" }
		);
		pdf.text(`${this.newOrder.customer.phoneNumber}`, 20, 4.8, {
			align: "right",
		});
		pdf.text(`${this.newOrder.customer.email}`, 20, 5.6, {
			align: "right",
		});
		//end-customer-info-block

		pdf.line(1, 6.2, 20, 6.2);
		//

		let y = 0;

		var bodyContent = [];

		for (const item in this.orderItems) {
			bodyContent.push({
				product: this.newOrder.orderItems[item].product.name,
				quantity:
					this.newOrder.orderItems[item].product.name.toString(),
				unitCost: `$${this.newOrder.orderItems[
					item
				].product.price.toFixed(2)}`,
				price: `$${(
					this.newOrder.orderItems[item].product.price *
					this.newOrder.orderItems[item].quantity
				).toFixed(2)}`,
			});
		}

		//version-2
		// bodyContent.push({
		// 	product: "-",
		// 	quantity: "-",
		// 	unitCost: `-`,
		// 	price: `-`,
		// });
		// bodyContent.pop();
		// this.newOrder.items.forEach((item: IOrderItemView) => {
		// 	bodyContent.push({
		// 		product: item.product.name,
		// 		quantity: item.product.name.toString(),
		// 		unitCost: `$${item.product.price.toFixed(2)}`,
		// 		price: `$${(item.product.price * item.quantity).toFixed(2)}`,
		// 	});
		// });
		//end-version-2

		autoTable(pdf, {
			startY: 7,
			showFoot: "lastPage",
			headStyles: { fillColor: "#ffaa4d", textColor: "#000000" },
			footStyles: {
				fillColor: "#ffa86b",
				textColor: "#000000",
			},
			columns: [
				{ header: "Product", dataKey: "product" },
				{ header: "Quantity", dataKey: "quantity" },
				{ header: "UnitCost", dataKey: "unitCost" },
				{ header: "Price", dataKey: "price" },
			],
			bodyStyles: { overflow: "visible", halign: "center" },
			styles: { lineWidth: 0.01, lineColor: "#000000" },
			head: [
				[
					{
						content: "Product",
						styles: { halign: "center" },
					},
					{
						content: "Quantity",
						styles: { halign: "center" },
					},
					{
						content: "UnitCost",
						styles: { halign: "center" },
					},
					{
						content: "Price",
						styles: { halign: "center" },
					},
				],
			],
			body: bodyContent,
			foot: [
				[
					{
						content: ["Total: "],
						colSpan: 3,
						rowSpan: 1,
						styles: { halign: "right" },
					},
					{
						content: `$${this.runningTotal.toFixed(2)}`,
						styles: { halign: "center" },
					},
				],
			],
			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		autoTable(pdf, {
			startY: y + 0.5,
			headStyles: { fillColor: "#ffaa4d", textColor: "#000000" },
			bodyStyles: { overflow: "visible" },
			columnStyles: {
				1: { halign: "right", minCellWidth: 4 },
				0: { minCellWidth: 10 },
			},
			styles: { lineWidth: 0.01, lineColor: "#000000" },

			head: [
				[
					{
						content: "Delivery address",
						styles: { halign: "left" },
					},
					{
						content: `${this.newOrder.customer.primaryAddress.addressLine1}`,
						styles: { halign: "right" },
					},
				],
			],
			body: [
				[
					"Postal code",
					`${this.newOrder.customer.primaryAddress.postalCode}`,
				],
				["City", `${this.newOrder.customer.primaryAddress.city}`],
				["Country", `${this.newOrder.customer.primaryAddress.country}`],
			],

			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		autoTable(pdf, {
			startY: y + 0.5,
			headStyles: { fillColor: "#ffaa4d", textColor: "#000000" },
			bodyStyles: { overflow: "visible" },
			columnStyles: {
				1: { halign: "right", minCellWidth: 4 },
				0: { minCellWidth: 10 },
			},
			styles: { lineWidth: 0.01, lineColor: "#000000" },

			head: [
				[
					{
						content: "Delivery Method",
						styles: { halign: "left" },
					},
					{
						content: `${this.newOrder.delivery.name}`,
						styles: { halign: "right" },
					},
				],
			],
			body: [["Price", `$${this.newOrder.delivery.price.toFixed(2)}`]],

			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		autoTable(pdf, {
			startY: y + 0.5,
			headStyles: { fillColor: "#ffaa4d", textColor: "#000000" },
			bodyStyles: { overflow: "visible" },
			columnStyles: {
				1: { halign: "right", minCellWidth: 4 },
				0: { minCellWidth: 10 },
			},
			styles: { lineWidth: 0.01, lineColor: "#000000" },

			head: [
				[
					{
						content: "Discount",
						styles: { halign: "left" },
					},
					{
						content: `${this.newOrder.discount.name}`,
						styles: { halign: "right" },
					},
				],
			],
			body: [["Percent", `${this.newOrder.discount.discountPercent}%`]],

			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		autoTable(pdf, {
			startY: y + 0.5,
			headStyles: { fillColor: "#ffaa4d", textColor: "#000000" },
			styles: { lineWidth: 0.01, lineColor: "#000000" },

			head: [
				[
					{
						content: "Payment Method",
						styles: { halign: "left", minCellWidth: 10 },
					},
					{
						content: `${this.newOrder.payment.name}`,
						styles: { halign: "right", minCellWidth: 4 },
					},
				],
			],

			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		autoTable(pdf, {
			startY: y + 0.5,
			headStyles: { fillColor: "#ffFFFF", textColor: "#000000" },
			styles: { lineWidth: 0.01, lineColor: "#000000" },

			head: [
				[
					{
						content: "Total price",
						styles: {
							halign: "left",
							fillColor: "#ffaa4d",
							valign: "middle",
						},
					},
					{
						content: `${this.runningTotal}`,
						styles: { halign: "center", valign: "middle" },
					},
					{
						content: `+ $${this.newOrder.delivery.price.toFixed(
							2
						)}`,
						styles: { halign: "center", valign: "middle" },
					},
					{
						content: `- ${this.newOrder.discount.discountPercent}%`,
						styles: { halign: "center", valign: "middle" },
					},
					{
						content: `= $${this.newOrder.totalPrice}`,
						styles: { halign: "center", fontSize: 12 },
					},
				],
			],
			didDrawPage: (HookData) => {
				y = HookData.cursor!.y;
			},
		});

		this.generatedPdfUri = pdf.output("datauristring");
		this.showGeneratedPdf = true;
		//pdf.output("dataurlnewwindow");
		//pdf.save("test.pdf");
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

::v-deep .dialog-wrapper.v-dialog {
	overflow-y: hidden !important;
}
</style>
