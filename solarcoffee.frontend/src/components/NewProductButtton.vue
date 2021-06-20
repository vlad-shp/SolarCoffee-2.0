<template>
	<v-dialog v-model="dialog" max-width="600px">
		<template v-slot:activator="{ on }">
			<v-btn
				elevation="0"
				color="secondary"
				v-on="on"
				:disabled="disabledButton"
			>
				Add New Product
				<v-icon right>mdi-plus</v-icon>
			</v-btn>
		</template>
		<v-card class="pa-5" v-if="dialog && product">
			<v-card-title>
				<v-row align="center" justify="space-between">
					New product
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
				<v-form v-model="formValid" v-if="product">
					<v-row>
						<v-col cols="12">
							<v-text-field
								v-model="product.name"
								:rules="[(v) => !!v || 'Field is required']"
								label="Name"
								clearable
								maxlength="100"
								counter
								prepend-inner-icon="mdi-format-title"
							/>

							<v-textarea
								v-model="product.description"
								label="Description"
								clearable
								counter
								maxlength="1000"
								prepend-inner-icon="mdi-card-text-outline"
							/>

							<v-checkbox
								v-model="product.isTaxable"
								label="Taxable"
							/>

							<v-text-field
								v-model.number="product.price"
								:rules="[
									(v) =>
										(!isNaN(parseInt(v)) && v > 0) ||
										'Price has to be more then 0',
								]"
								label="Price"
								type="number"
								prepend-inner-icon="mdi-cash-multiple"
							/>

							<v-row>
								<v-col cols="6">
									<v-text-field
										v-model.number="idealQuantity"
										:rules="[
											(v) =>
												(!isNaN(parseInt(v)) &&
													v > 0) ||
												'Ideal quantity has to be more then 0',
										]"
										label="Ideal quantity"
										prepend-inner-icon="mdi-alpha-i-box-outline"
									/>
								</v-col>
								<v-col cols="6">
									<v-text-field
										v-model.number="quantityOnHand"
										:rules="[
											(v) =>
												(!isNaN(parseInt(v)) &&
													v >= 0) ||
												'Quantity on hand has to be more then 0',
										]"
										label="Quantity on hand"
										prepend-inner-icon="mdi-alpha-q-box-outline"
									/>
								</v-col>
							</v-row>
						</v-col>
					</v-row>
				</v-form>
			</v-card-text>
		</v-card>
	</v-dialog>
</template>

<script lang="ts">
import { Component, Vue, Watch, Inject, Prop } from "vue-property-decorator";
import IProduct from "@/models/product";
import ProductService from "@/services/product-service";

@Component
export default class NewProductButton extends Vue {
	@Inject() readonly productService!: ProductService;

	@Prop({ required: true })
	disabledButton!: boolean;

	dialog = false;
	formValid = false;

	product: IProduct | null = null;

	idealQuantity = 0;
	quantityOnHand = 0;

	@Watch("dialog")
	onDialogStateChange(newDialogState: boolean): void {
		if (newDialogState) {
			this.product = {
				id: 0,
				name: "",
				createdOn: new Date(),
				updatedOn: new Date(),
				description: "",
				price: 0,
				isTaxable: false,
				isArchived: false,
			};
		} else {
			this.product = null;
			this.idealQuantity = 0;
			this.quantityOnHand = 0;
		}
	}

	async addNewCustomer(): Promise<void> {
		if (
			this.product != null &&
			this.formValid &&
			this.product.price > 0 &&
			this.quantityOnHand >= 0 &&
			this.idealQuantity > 0
		) {
			await this.productService.addProduct(
				this.product,
				this.idealQuantity,
				this.quantityOnHand
			);

			this.$emit("productAdded");

			this.dialog = false;

			this.product = null;
		}
	}
}
</script>

<style scoped></style>
