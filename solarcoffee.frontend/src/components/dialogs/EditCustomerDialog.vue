<template>
	<v-dialog v-model="dialog" max-width="800px">
		<v-card v-if="refreshedCustomer" class="py-4">
			<v-card-title> Edit customer details </v-card-title>
			<v-card-text>
				<v-form v-model="formValid">
					<v-row>
						<v-col cols="6">
							<v-text-field
								v-model="refreshedCustomer.firstName"
								:rules="[(v) => !!v || 'Field is required']"
								label="First name"
								clearable
								maxlength="100"
								counter
								prepend-inner-icon="mdi-account"
							/>

							<v-text-field
								v-model="refreshedCustomer.email"
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
								v-model="
									refreshedCustomer.primaryAddress
										.addressLine1
								"
								:rules="[(v) => !!v || 'Field is required']"
								label="Address Line 1"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-map-marker"
							/>

							<v-text-field
								v-model="refreshedCustomer.primaryAddress.city"
								:rules="[(v) => !!v || 'Field is required']"
								label="City"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-city"
							/>

							<v-text-field
								v-model="
									refreshedCustomer.primaryAddress.postalCode
								"
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
								v-model="refreshedCustomer.lastName"
								:rules="[(v) => !!v || 'Field is required']"
								label="Last name"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-account"
							/>

							<v-text-field
								v-model="refreshedCustomer.phoneNumber"
								:rules="[(v) => !!v || 'Field is required']"
								label="Phone number"
								clearable
								counter
								maxlength="25"
								prepend-inner-icon="mdi-phone"
							/>

							<v-text-field
								v-model="
									refreshedCustomer.primaryAddress
										.addressLine2
								"
								label="Address Line 2"
								clearable
								counter
								maxlength="100"
								prepend-inner-icon="mdi-map-marker"
							/>

							<v-text-field
								v-model="
									refreshedCustomer.primaryAddress.country
								"
								:rules="[(v) => !!v || 'Field is required']"
								label="Country"
								clearable
								counter
								maxlength="32"
								prepend-inner-icon="mdi-home-city"
							/>

							<v-text-field
								v-model="refreshedCustomer.primaryAddress.state"
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
			<v-divider />

			<v-card-actions>
				<v-spacer />
				<v-btn color="blue darken-1" text @click="dialog = false">
					Close
				</v-btn>
				<v-btn
					color="blue darken-1"
					text
					:disabled="!formValid"
					@click="onSave"
				>
					Save
				</v-btn>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>

<script lang="ts">
import ICustomer from "@/models/customer";
import { Component, Vue, Prop, Watch, PropSync } from "vue-property-decorator";

@Component
export default class EditCustomerDialog extends Vue {
	@Prop({ required: true }) customer!: ICustomer;

	@PropSync("editDialogVisible", { required: true })
	dialog!: boolean;

	refreshedCustomer: ICustomer | null = {
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

	formValid = false;

	@Watch("dialog")
	onDialogChange(newValue: boolean): void {
		if (newValue && this.customer.id) {
			this.refreshedCustomer = {
				id: this.customer.id,
				createdOn: this.customer.createdOn,
				updatedOn: new Date(),
				email: this.customer.email,
				phoneNumber: this.customer.phoneNumber,
				primaryAddress: {
					id: this.customer.primaryAddress.id,
					createdOn: this.customer.primaryAddress.createdOn,
					updatedOn: new Date(),
					addressLine1: this.customer.primaryAddress.addressLine1,
					addressLine2: this.customer.primaryAddress.addressLine2,
					city: this.customer.primaryAddress.city,
					state: this.customer.primaryAddress.state,
					postalCode: this.customer.primaryAddress.postalCode,
					country: this.customer.primaryAddress.country,
				},
				firstName: this.customer.firstName,
				lastName: this.customer.lastName,
			};
		} else if (!newValue) {
			this.refreshedCustomer = null;
		}
	}

	onSave(): void {
		this.dialog = false;
		this.$emit("customerEdited", this.refreshedCustomer);
	}
}
</script>
