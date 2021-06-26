import { INewOrder } from "@/models/request/order/new-order";
import IOrderView from "@/models/view/order-view";
import axios from "axios";

/**
 * Order Service
 */
export default class OrderService {
	API_URL = process.env.VUE_APP_API_URL;

	public async addNewOrder(newOrder: INewOrder): Promise<boolean> {
		const result = await axios.post<boolean>(
			`${this.API_URL}/newOrder`,
			newOrder
		);
		return result.data;
	}

	public async getOrders(): Promise<IOrderView[]> {
		const result = await axios.get<IOrderView[]>(`${this.API_URL}/order`);
		return result.data;
	}
}
