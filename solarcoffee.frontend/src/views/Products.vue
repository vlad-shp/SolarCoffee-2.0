<template>
	<v-container justify="center">
		<v-card class="pa-12">
			<v-row align="center" justify="space-between">
				<span class="black--text text-h5">Products</span>
				<new-product-button
					:disabledButton="!dataLoaded"
					@productAdded="addedNewProduct"
				/>
			</v-row>
		</v-card>
		<v-card class="pa-12 mt-5">
			<v-data-table
				:headers="productsHeaders"
				:items="products"
				fixed-header
				:loading="!dataLoaded"
				v-resize="onResize"
			>
				<template v-slot:[`item.isArchived`]="{ item }">
					<v-simple-checkbox
						v-model="item.isArchived"
						disabled
					></v-simple-checkbox>
				</template>
				<template v-slot:[`item.isTaxable`]="{ item }">
					<v-simple-checkbox
						v-model="item.isTaxable"
						disabled
					></v-simple-checkbox>
				</template>
				<template v-slot:[`item.actions`]="{ item }">
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
										>mdi-square-edit-outline</v-icon
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
									@click="archive(item.id)"
									:disabled="item.isArchived"
									elevation="0"
									icon
								>
									<v-icon color="orange"
										>mdi-archive-arrow-down</v-icon
									>
								</v-btn>
							</template>
							<span>Archive</span>
						</v-tooltip>
					</v-row>
				</template>
			</v-data-table>
		</v-card>

		<edit-product-dialog
			:editDialogVisible.sync="editDialogVisible"
			:product="currentProduct"
			@productEdited="productEdited"
		/>
	</v-container>
</template>

<script lang="ts">
import { Component, Vue, Inject } from "vue-property-decorator";
import IProduct from "@/models/request/product";
import { DataTableHeader } from "vuetify";
import ProductService from "@/services/product-service";
import NewProductButton from "@/components/NewProductButtton.vue";
import EditProductDialog from "@/components/dialogs/EditProductDialog.vue";
import Loading from "@/components/Loading.vue";

@Component({ components: { NewProductButton, EditProductDialog, Loading } })
export default class Products extends Vue {
	@Inject() readonly productService!: ProductService;

	dataLoaded = false;

	products: IProduct[] = [];

	currentProduct: IProduct | null = null;

	editDialogVisible = false;

	get productsHeaders(): DataTableHeader[] {
		return [
			{ text: "Name", value: "name", align: "center" },
			{ text: "Price", value: "price", align: "center" },
			{ text: "Taxable", value: "isTaxable", align: "center" },
			{ text: "Archived", value: "isArchived", align: "center" },
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

	openEditDialog(product: IProduct): void {
		this.currentProduct = product;
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

	async initialize(): Promise<void> {
		this.dataLoaded = false;
		this.products = await this.productService.getProducts();
		this.dataLoaded = true;
	}

	async addedNewProduct(): Promise<void> {
		await this.initialize();
	}

	async productEdited(product: IProduct): Promise<void> {
		await this.productService.refreshProduct(product.id, product);

		const editedProductIndex = this.products.findIndex(
			(p) => p.id == product.id
		);

		if (editedProductIndex > -1) {
			this.$set(this.products, editedProductIndex, product);
		}
	}

	async archive(index: number): Promise<void> {
		await this.productService.archiveProduct(index);

		const editedProduct = this.products.find((p) => p.id == index);

		if (editedProduct != null) {
			editedProduct.isArchived = true;
		}
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
