import axios from "axios";

/**
 * Order Service
 */
export default class OrderService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getOrders(): Promise<any> {
		const result = await axios.get<any>(`${this.API_URL}/order/`);
		return result.data;
	}
}
