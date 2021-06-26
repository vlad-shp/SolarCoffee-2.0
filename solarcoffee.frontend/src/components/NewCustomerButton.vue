<template>
	<v-dialog v-model="dialog" max-width="600px">
		<template v-slot:activator="{ on }">
			<v-btn
				elevation="0"
				color="secondary"
				v-on="on"
				:disabled="disabledButton"
			>
				Add New Customer
				<v-icon right>mdi-plus</v-icon>
			</v-btn>
		</template>
		<v-card class="pa-5" v-if="dialog && customer">
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
			<v-card-text>
				<v-form v-model="formValid">
					<v-row>
						<v-col cols="6">
							<v-text-field
								v-model="customer.firstName"
								:rules="[(v) => !!v || 'Field is required']"
								label="First name"
								clearable
								maxlength="100"
								counter
								prepend-inner-icon="mdi-account"
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
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-email"
							/>

							<v-text-field
								v-model="customer.primaryAddress.addressLine1"
								:rules="[(v) => !!v || 'Field is required']"
								label="Address Line 1"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-map-marker"
							/>

							<v-text-field
								v-model="customer.primaryAddress.city"
								:rules="[(v) => !!v || 'Field is required']"
								label="City"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-city"
							/>

							<v-text-field
								v-model="customer.primaryAddress.postalCode"
								:rules="[(v) => !!v || 'Field is required']"
								label="Postal code"
								clearable
								counter
								maxlength="10"
								prepend-inner-icon="mdi-office-building-marker"
							/>
						</v-col>

						<v-col cols="6">
							<v-text-field
								v-model="customer.lastName"
								:rules="[(v) => !!v || 'Field is required']"
								label="Last name"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-account"
							/>

							<v-text-field
								v-model="customer.phoneNumber"
								:rules="[(v) => !!v || 'Field is required']"
								label="Phone number"
								clearable
								counter
								maxlength="25"
								prepend-inner-icon="mdi-phone"
							/>

							<v-text-field
								v-model="customer.primaryAddress.addressLine2"
								label="Address Line 2"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-map-marker"
							/>

							<v-text-field
								v-model="customer.primaryAddress.country"
								:rules="[(v) => !!v || 'Field is required']"
								label="Country"
								clearable
								counter
								maxlength="32"
								prepend-inner-icon="mdi-home-city"
							/>

							<v-text-field
								v-model="customer.primaryAddress.state"
								:rules="[(v) => !!v || 'Field is required']"
								label="State"
								maxlength="4"
								clearable
								counter
								prepend-inner-icon="mdi-sign-real-estate"
							/>
						</v-col>
					</v-row>
				</v-form>
			</v-card-text>
		</v-card>
	</v-dialog>
</template>

<script lang="ts">
import ICustomer from "@/models/request/customer/customer";
import { Component, Vue, Watch, Inject, Prop } from "vue-property-decorator";
import CustomerService from "@/services/customer-service";

@Component
export default class NewCustomerButton extends Vue {
	@Inject() readonly customerService!: CustomerService;

	@Prop({ required: true })
	disabledButton!: boolean;

	dialog = false;
	formValid = false;

	customer: ICustomer | null = null;

	@Watch("dialog")
	onDialogStateChange(newDialogState: boolean): void {
		if (newDialogState) {
			this.customer = {
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
		} else {
			this.customer = null;
		}
	}

	async addNewCustomer(): Promise<void> {
		if (this.customer != null && this.formValid) {
			await this.customerService.addCustomer(this.customer);

			this.$emit("customerAdded");

			this.dialog = false;

			this.customer = null;
		}
	}
}
</script>

<style scoped></style>
