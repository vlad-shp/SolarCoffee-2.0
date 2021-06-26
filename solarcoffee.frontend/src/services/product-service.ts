import IProduct from "@/models/request/product";
import axios from "axios";

/**
 * Product Service
 */
export default class ProductService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getProducts(): Promise<IProduct[]> {
		const result = await axios.get<IProduct[]>(`${this.API_URL}/product/`);
		return result.data;
	}

	public async addProduct(
		newProduct: IProduct,
		idealQuantityForSale: number,
		startQuantity: number
	): Promise<IProduct> {
		const result = await axios.post<IProduct>(
			`${this.API_URL}/product?idealQuantityForSale=${idealQuantityForSale}&startQuantity=${startQuantity}`,
			newProduct
		);
		return result.data;
	}

	public async refreshProduct(
		productId: number,
		refreshedProduct: IProduct
	): Promise<IProduct> {
		const result = await axios.patch<IProduct>(
			`${this.API_URL}/product/${productId}`,
			refreshedProduct
		);
		return result.data;
	}

	public async archiveProduct(productId: number): Promise<boolean> {
		const result = await axios.patch<boolean>(
			`${this.API_URL}/product/${productId}/archive`
		);
		return result.data;
	}
}
