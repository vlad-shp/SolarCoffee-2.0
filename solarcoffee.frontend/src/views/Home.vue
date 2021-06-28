<template>
	<div>
		<v-row no-gutters justify="center">
			<loading msg="Loading items..." v-if="!dataLoaded" />
			<v-col
				v-else
				cols="12"
				sm="6"
				md="3"
				v-for="product in products"
				:key="product.id"
				class="pa-4"
			>
				<v-card max-width="374">
					<template slot="progress">
						<v-progress-linear
							color="deep-purple"
							height="10"
							indeterminate
						></v-progress-linear>
					</template>

					<v-img
						height="200"
						src="../assets/logo.png"
						style="height: 100%"
					></v-img>

					<v-card-title>{{ product.name }}</v-card-title>

					<v-card-text>
						<v-row align="center" class="mx-0">
							<v-rating
								:value="4.5"
								color="amber"
								dense
								half-increments
								readonly
								size="14"
							></v-rating>

							<div class="grey--text ms-4">4.5 (413)</div>
						</v-row>

						<div class="my-4 text-subtitle-1">
							{{ product.price | price }}
						</div>

						<div>
							{{ product.description }}
						</div>
					</v-card-text>

					<v-divider class="mx-4"></v-divider>

					<v-card-actions>
						<v-btn color="primary" text @click="addToCart">
							Add to cart
						</v-btn>
					</v-card-actions>
				</v-card>
			</v-col>
		</v-row>
	</div>
</template>

<script lang="ts">
import IProduct from "@/models/request/product";
import ProductService from "@/services/product-service";
import { Component, Vue, Inject } from "vue-property-decorator";
import Loading from "@/components/Loading.vue";

@Component({ components: { Loading } })
export default class Home extends Vue {
	@Inject() readonly productService!: ProductService;

	products: IProduct[] = [];

	selection = 1;
	dataLoaded = false;

	async created(): Promise<void> {
		await this.initialize();
	}

	async initialize(): Promise<void> {
		this.dataLoaded = false;
		this.products = await this.productService.getProducts();
		this.dataLoaded = true;
	}

	addToCart(): void {
		//
	}
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
</style>
