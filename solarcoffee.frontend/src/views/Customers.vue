<template>
	<v-container>
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Customers</span>
				<v-btn
					elevation="0"
					color="secondary"
					@click="
						addNewCustomerFormVisible = !addNewCustomerFormVisible
					"
				>
					Add New Customer
					<v-icon right>mdi-plus</v-icon>
				</v-btn>
			</v-row>
		</v-card>
		<add-new-customer
			v-if="addNewCustomerFormVisible"
			@customerAdded="addedNewCustomer"
		/>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="customersHeaders"
				:items="customers"
				fixed-header
				v-resize="onResize"
			>
				<template v-slot:[`item.actions`]="{ item }">
					<v-row justify="center">
						<v-btn @click="openEditDialog(item)" elevation="0" icon>
							<v-icon color="#149000">mdi-account-edit</v-icon>
						</v-btn>
						<v-btn @click="remove(item.id)" elevation="0" icon>
							<v-icon color="red">mdi-account-remove</v-icon>
						</v-btn>
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
import { Component, Vue } from "vue-property-decorator";
import { DataTableHeader } from "vuetify";
import AddNewCustomer from "@/components/AddNewCustomer.vue";
import EditCustomerDialog from "@/components/dialogs/EditCustomerDialog.vue";

import CustomerService from "@/services/customer-service";

const customerService = new CustomerService();

@Component({ components: { AddNewCustomer, EditCustomerDialog } })
export default class Customers extends Vue {
	customers: ICustomer[] = [];

	currentCustomer: ICustomer | null = null;

	editDialogVisible = false;
	addNewCustomerFormVisible = false;

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
		this.customers = await customerService.getCustomers();
	}

	async addedNewCustomer(): Promise<void> {
		this.addNewCustomerFormVisible = false;

		await this.initialize();
	}

	async remove(index: number): Promise<void> {
		await customerService.deleteCustomer(index);
		await this.initialize();
	}

	async customerEdited(customer: ICustomer): Promise<void> {
		await customerService.refreshCustomer(customer.id, customer);
		await this.initialize();
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
