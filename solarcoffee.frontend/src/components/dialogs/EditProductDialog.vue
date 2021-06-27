<template>
	<v-dialog v-model="dialog" max-width="800px">
		<v-card v-if="refreshedProduct" class="py-4">
			<v-card-title> Edit product details </v-card-title>
			<v-card-text>
				<v-form v-model="formValid">
					<v-row>
						<v-col cols="12">
							<v-text-field
								v-model="refreshedProduct.name"
								:rules="[(v) => !!v || 'Field is required']"
								label="Title"
								clearable
								maxlength="100"
								counter
								prepend-inner-icon="mdi-format-title"
							/>

							<v-textarea
								v-model="refreshedProduct.description"
								label="Description"
								clearable
								counter
								maxlength="1000"
								prepend-inner-icon="mdi-card-text-outline"
							/>

							<v-row justify="space-around">
								<v-checkbox
									v-model="refreshedProduct.isTaxable"
									label="Taxable"
								/>
								<v-checkbox
									v-model="refreshedProduct.isArchived"
									label="Archived"
								/>
							</v-row>
							<v-text-field
								v-model.number="refreshedProduct.price"
								:rules="[
									(v) =>
										(!isNaN(parseInt(v)) && v > 0) ||
										'Price has to be more then 0',
								]"
								label="Price"
								type="number"
								prepend-inner-icon="mdi-cash-multiple"
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
import IProduct from "@/models/request/product";
import { Component, Vue, Prop, Watch, PropSync } from "vue-property-decorator";

@Component
export default class EditProductDialog extends Vue {
	@Prop({ required: true }) product!: IProduct;

	@PropSync("editDialogVisible", { required: true })
	dialog!: boolean;

	refreshedProduct: IProduct | null = null;

	formValid = false;

	@Watch("dialog")
	onDialogChange(newValue: boolean): void {
		if (newValue && this.product) {
			this.refreshedProduct = {
				id: this.product.id,
				createdOn: this.product.createdOn,
				updatedOn: new Date(),
				name: this.product.name,
				description: this.product.description,
				price: this.product.price,
				isTaxable: this.product.isTaxable,
				isArchived: this.product.isArchived,
			};
		} else if (!newValue) {
			this.refreshedProduct = null;
		}
	}

	onSave(): void {
		this.dialog = false;
		this.$emit("productEdited", this.refreshedProduct);
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
