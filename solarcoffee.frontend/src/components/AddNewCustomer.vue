<template>
	<v-container>
		<v-card class="pa-12">
			<v-card-title>
				<v-row align="center" justify="space-between">
					New customer
					<v-btn
						:disabled="!formValid"
						elevation="0"
						color="secondary"
						@click="addNewCustomer"
					>
						<v-icon color="white">mdi-plus</v-icon>
					</v-btn>
				</v-row>
			</v-card-title>
			<v-card-text class="pl-0">
				<v-form v-model="formValid">
					<v-text-field
						v-model="customer.firstName"
						:rules="[(v) => !!v || 'Field is required']"
						label="First name"
					/>

					<v-text-field
						v-model="customer.lastName"
						:rules="[(v) => !!v || 'Field is required']"
						label="Last name"
					/>

					<v-text-field
						v-model="customer.email"
						:rules="[
							(v) =>
								!v ||
								/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(
									v
								) ||
								'E-mail must be valid',
						]"
						label="Email"
					/>

					<v-text-field
						v-model="customer.phoneNumber"
						:rules="[(v) => !!v || 'Field is required']"
						label="Phone number"
					/>

					<v-text-field
						v-model="customer.primaryAddress.addressLine1"
						:rules="[(v) => !!v || 'Field is required']"
						label="Address Line 1"
					/>

					<v-text-field
						v-model="customer.primaryAddress.addressLine2"
						label="Address Line 2"
					/>

					<v-text-field
						v-model="customer.primaryAddress.city"
						:rules="[(v) => !!v || 'Field is required']"
						label="City"
					/>

					<v-text-field
						v-model="customer.primaryAddress.country"
						:rules="[(v) => !!v || 'Field is required']"
						label="Country"
					/>

					<v-text-field
						v-model="customer.primaryAddress.postalCode"
						:rules="[(v) => !!v || 'Field is required']"
						label="Postal code"
					/>

					<v-text-field
						v-model="customer.primaryAddress.state"
						:rules="[(v) => !!v || 'Field is required']"
						label="State"
						maxlength="4"
						counter
					/>
				</v-form>
			</v-card-text>
		</v-card>
	</v-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ICustomer from "@/models/customer";
import CustomerService from "@/services/customer-service";

const customerService = new CustomerService();

@Component
export default class AddNewCustomer extends Vue {
	formValid = true;

	customer: ICustomer = {
		id: 0,
		firstName: "",
		lastName: "",
		email: "",
		phoneNumber: "",
		createdOn: new Date(),
		updatedOn: new Date(),
		primaryAddress: {
			id: 0,
			updatedOn: new Date(),
			createdOn: new Date(),
			addressLine1: "",
			addressLine2: "",
			city: "",
			state: "",
			postalCode: "",
			country: "",
		},
	};

	async addNewCustomer(): Promise<void> {
		await customerService.addCustomer(this.customer);
		this.$emit("customerAdded");
	}
}
</script>

<style scoped></style>
