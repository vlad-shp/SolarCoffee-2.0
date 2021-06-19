<template>
	<v-container>
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Customers</span>
				<new-customer-button
					:disabledButton="!dataLoaded"
					@customerAdded="addedNewCustomer"
				/>
			</v-row>
		</v-card>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="customersHeaders"
				:items="customers"
				:loading="!dataLoaded"
				fixed-header
				v-resize="onResize"
			>
				<template #[`item.actions`]="{ item }">
					<v-row justify="center">
						<v-tooltip top>
							<template v-slot:activator="{ on, attrs }">
								<v-btn
									v-on="on"
									v-bind="attrs"
									@click="openEditDialog(item)"
									elevation="0"
									icon
								>
									<v-icon color="#149000"
										>mdi-account-edit</v-icon
									>
								</v-btn>
							</template>
							<span>Edit</span>
						</v-tooltip>
						<v-tooltip top>
							<template v-slot:activator="{ on, attrs }">
								<v-btn
									v-on="on"
									v-bind="attrs"
									@click="remove(item.id)"
									elevation="0"
									icon
								>
									<v-icon color="red"
										>mdi-account-remove</v-icon
									>
								</v-btn>
							</template>
							<span>Remove</span>
						</v-tooltip>
					</v-row>
				</template>
			</v-data-table>
		</v-card>

		<edit-customer-dialog
			:editDialogVisible.sync="editDialogVisible"
			:customer="currentCustomer"
			@customerEdited="customerEdited"
		/>
	</v-container>
</template>

<script lang="ts">
import ICustomer from "@/models/customer";
import { Component, Vue, Inject } from "vue-property-decorator";
import { DataTableHeader } from "vuetify";
import NewCustomerButton from "@/components/NewCustomerButton.vue";
import EditCustomerDialog from "@/components/dialogs/EditCustomerDialog.vue";

import CustomerService from "@/services/customer-service";

@Component({ components: { NewCustomerButton, EditCustomerDialog } })
export default class Customers extends Vue {
	@Inject() readonly customerService!: CustomerService;

	customers: ICustomer[] = [];

	currentCustomer: ICustomer | null = null;

	editDialogVisible = false;
	dataLoaded = false;

	get customersHeaders(): DataTableHeader[] {
		return [
			{ text: "First name", value: "firstName", align: "center" },
			{ text: "Last name", value: "lastName", align: "center" },
			{ text: "Email", value: "email", align: "center" },
			{ text: "Phone number", value: "phoneNumber", align: "center" },
			{ text: "City", value: "primaryAddress.city", align: "center" },
			{
				text: "Postal code",
				value: "primaryAddress.postalCode",
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
	async created(): Promise<void> {
		await this.initialize();
	}

	async initialize(): Promise<void> {
		this.dataLoaded = false;
		this.customers = await this.customerService.getCustomers();
		this.dataLoaded = true;
	}

	async addedNewCustomer(): Promise<void> {
		await this.initialize();
	}

	async remove(index: number): Promise<void> {
		await this.customerService.deleteCustomer(index);
		await this.initialize();
	}

	async customerEdited(customer: ICustomer): Promise<void> {
		await this.customerService.refreshCustomer(customer.id, customer);

		const editedCustomerIndex = this.customers.findIndex(
			(c) => c.id == customer.id
		);

		if (editedCustomerIndex > -1) {
			this.$set(this.customers, editedCustomerIndex, customer);
		}
	}

	openEditDialog(customer: ICustomer): void {
		this.currentCustomer = customer;
		this.editDialogVisible = true;
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
