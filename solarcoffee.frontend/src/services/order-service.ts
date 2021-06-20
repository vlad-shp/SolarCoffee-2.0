import IOrder from "@/models/order";
import axios from "axios";

/**
 * Order Service
 */
export default class OrderService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getOrders(): Promise<IOrder[]> {
		const result = await axios.get<IOrder[]>(`${this.API_URL}/order/`);
		return result.data;
	}
}
