<template>
	<v-container>
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Orders</span>
				<div>
					<new-order-button
						:disabledButton="!dataLoaded"
						:newOrderId="newOrderId"
						@addedNewOrder="initialize"
					/>
					<order-settings-button :disabledButton="!dataLoaded" />
				</div>
			</v-row>
		</v-card>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="ordersHeader"
				:items="orders"
				:single-expand="singleExpand"
				:expanded.sync="expanded"
				:loading="!dataLoaded"
				show-expand
				@click:row="(item, slot) => slot.expand(!slot.isExpanded)"
			>
				<template #top>
					<v-toolbar flat>
						<v-switch
							v-model="singleExpand"
							label="Single expand"
						></v-switch>
					</v-toolbar>
				</template>
				<template #[`item.customer`]="{ item }">
					{{
						`${item.customer.id}, ${item.customer.firstName} ${item.customer.lastName}`
					}}
				</template>
				<template #[`item.createdOn`]="{ item }">
					{{ convertDate(item.createdOn) }}
				</template>
				<template #[`item.orderStatus`]="{ item }">
					{{ convertStatusToString(item.orderStatus) }}
				</template>
				<template #[`item.totalPrice`]="{ item }">
					{{ item.totalPrice | price }}
				</template>
				<template #[`item.actions`]="{ item }">
					{{ item.id }}
				</template>
				<template #expanded-item="{ headers, item }">
					<td :colspan="headers.length" class="pa-0">
						<v-data-table
							:headers="orderItemHeader"
							:items="item.orderItems"
							:hide-default-footer="true"
						>
							<template #[`item.lineTotal`]="{ item }">
								{{
									(item.product.price * item.quantity) | price
								}}
							</template>
						</v-data-table>
					</td>
				</template>
			</v-data-table>
		</v-card>

		<!-- <receive-shipment-dialog
			:receiveShipmentDialogVisible.sync="receiveShipmentDialogVisible"
			:shipment="shipment"
			:productName="shipmentProductName"
			@saveReceiveShipment="saveReceiveShipment"
		/> -->
	</v-container>
</template>

<script lang="ts">
import { Component, Vue, Inject } from "vue-property-decorator";
import { DataTableHeader } from "vuetify";
import OrderService from "@/services/order-service";
import NewOrderButton from "@/components/NewOrderButton.vue";
import OrderSettingsButton from "@/components/OrderSettingsButton.vue";
import IOrderView from "@/models/view/order-view";
import moment from "moment";
import { OrderStatus } from "@/models/request/order/new-order";

@Component({ components: { NewOrderButton, OrderSettingsButton } })
export default class Order extends Vue {
	@Inject() readonly orderService!: OrderService;

	dataLoaded = false;
	singleExpand = true;

	orders: IOrderView[] = [];

	newOrderId = 1;

	expanded = [];

	get ordersHeader(): DataTableHeader[] {
		return [
			{ text: "Customer", value: "customer", align: "center" },
			{ text: "Payment", value: "payment.name", align: "center" },
			{ text: "Discount", value: "discount.name", align: "center" },
			{
				text: "Delivery",
				value: "delivery.name",
				align: "center",
			},
			{ text: "Status", value: "orderStatus", align: "center" },
			{ text: "Total Price", value: "totalPrice", align: "center" },
			{ text: "Created", value: "createdOn", align: "center" },
			{
				text: "Actions",
				value: "actions",
				align: "center",
				sortable: false,
			},
		];
	}

	get orderItemHeader(): DataTableHeader[] {
		return [
			{
				text: "Product Id",
				value: "product.id",
				align: "center",
				divider: true,
			},
			{
				text: "Product Name",
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
		];
	}

	convertDate(date: string): string {
		return moment(Date.parse(date)).format("MMMM Do YYYY");
	}

	convertStatusToString(status: number): string {
		return OrderStatus[status];
	}

	async created(): Promise<void> {
		await this.initialize();
	}

	async initialize(): Promise<void> {
		this.dataLoaded = false;
		this.orders = await this.orderService.getOrders();
		this.dataLoaded = true;

		if (this.orders.length > 0) {
			this.newOrderId = this.orders[this.orders.length - 1].id;
		}
	}

	openReceiveShipmentDialog(item: unknown): void {
		//
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

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
