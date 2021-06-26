<template>
	<v-container>
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Orders</span>
				<div>
					<new-order-button
						:disabledButton="!dataLoaded"
						:newOrderId="newOrderId"
					/>
					<order-settings-button :disabledButton="!dataLoaded" />
				</div>
			</v-row>
		</v-card>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="ordersHeader"
				:items="orders"
				:loading="!dataLoaded"
				fixed-header
				v-resize="onResize"
				sort-by="product.name"
			>
				<template #[`item.actions`]="{ item }">
					<v-row justify="center">
						<v-tooltip top>
							<template v-slot:activator="{ on, attrs }">
								<v-btn
									v-on="on"
									v-bind="attrs"
									@click="openReceiveShipmentDialog(item)"
									elevation="0"
									icon
								>
									<v-icon color="#149000"
										>mdi-truck-delivery-outline</v-icon
									>
								</v-btn>
							</template>
							<span>Receive shipment</span>
						</v-tooltip>
					</v-row>
				</template>
				<template #[`item.createdOn`]="{ item }">
					{{ convertDate(item.createdOn) }}
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

@Component({ components: { NewOrderButton, OrderSettingsButton } })
export default class Order extends Vue {
	@Inject() readonly orderService!: OrderService;

	dataLoaded = false;

	orders: IOrderView[] = [];

	newOrderId = 1;

	get ordersHeader(): DataTableHeader[] {
		return [
			{ text: "Customer", value: "customer.id", align: "center" },
			{ text: "Payment", value: "payment.id", align: "center" },
			{ text: "Discount", value: "discount.id", align: "center" },
			{
				text: "Delivery",
				value: "delivery.id",
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

	convertDate(date: string): string {
		return moment(Date.parse(date)).format("MMMM Do YYYY");
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

<style scoped></style>
