<template>
	<v-container>
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Inventory</span>
				<v-checkbox
					:disabled="!dataLoaded"
					label="Show archived"
					v-model="showArchived"
					:color="showArchivedCheckBoxColor"
				/>
			</v-row>
		</v-card>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="inventoryHeaders"
				:items="filteredItems"
				:loading="!dataLoaded"
				fixed-header
				v-resize="onResize"
				sort-by="product.name"
			>
				<template v-slot:[`item.product.name`]="{ item }">
					<v-chip :color="getColor(item)" dark>
						{{ item.product.name }}
					</v-chip>
				</template>
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
			</v-data-table>
		</v-card>

		<receive-shipment-dialog
			:receiveShipmentDialogVisible.sync="receiveShipmentDialogVisible"
			:shipment="shipment"
			:productName="shipmentProductName"
			@saveReceiveShipment="saveReceiveShipment"
		/>
	</v-container>
</template>

<script lang="ts">
import IInventory from "@/models/inventory";
import { IShipment } from "@/models/shipment";
import InventoryService from "@/services/inventory-service";
import { Component, Vue, Inject } from "vue-property-decorator";
import { DataTableHeader } from "vuetify";
import ReceiveShipmentDialog from "@/components/dialogs/ReceiveShipmentDialog.vue";

@Component({ components: { ReceiveShipmentDialog } })
export default class Inventory extends Vue {
	@Inject() readonly inventoryService!: InventoryService;

	get filteredItems(): IInventory[] {
		if (this.showArchived) {
			return this.inventoryItems;
		} else {
			return this.inventoryItems.filter((i) => {
				return (
					!this.inventoryItems ||
					i.product.isArchived === this.showArchived
				);
			});
		}
	}

	shipment: IShipment | null = null;
	shipmentProductName = "";

	dataLoaded = false;
	showArchived = false;
	receiveShipmentDialogVisible = false;

	inventoryItems: IInventory[] = [];

	get inventoryHeaders(): DataTableHeader[] {
		return [
			{ text: "Product", value: "product.name", align: "center" },
			{ text: "Ideal quantity", value: "idealQuantity", align: "center" },
			{
				text: "Quantity on hand",
				value: "quantityOnHand",
				align: "center",
			},
			{
				text: "Actions",
				value: "actions",
				align: "center",
				sortable: false,
			},
		];
	}

	get showArchivedCheckBoxColor(): string {
		if (this.showArchived) {
			return "black";
		}
		return "white";
	}

	async created(): Promise<void> {
		await this.initialize();
	}

	async initialize(): Promise<void> {
		this.dataLoaded = false;

		this.inventoryItems = await this.inventoryService.getInventory();

		this.dataLoaded = true;
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

	getColor(item: IInventory): string {
		if (item.product.isArchived === true) {
			return "black";
		}
		if (item.quantityOnHand === 0) {
			return "red";
		}
		if (item.quantityOnHand < item.idealQuantity) {
			return "orange";
		}

		return "green";
	}

	openReceiveShipmentDialog(item: IInventory): void {
		this.shipment = {
			productId: item.id,
			adjustment: 0,
		};
		this.shipmentProductName = item.product.name;
		this.receiveShipmentDialogVisible = true;
	}

	async saveReceiveShipment(shipment: IShipment): Promise<void> {
		await this.inventoryService.updateInventory(shipment);

		const item = this.inventoryItems.filter(
			(ii) => ii.product.id === shipment.productId
		)[0];
		item.quantityOnHand += +shipment.adjustment;
	}
}
</script>

<style scoped></style>
