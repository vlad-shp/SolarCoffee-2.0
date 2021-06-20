import IOrderSettings from "@/models/order-settings";
import IPayment from "@/models/payment";
import IDelivery from "@/models/delivery";
import IDiscount from "@/models/discount";
import axios from "axios";

/**
 * Order Settings Service
 */
export default class OrderSettingsService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getOrderSettings(): Promise<IOrderSettings> {
		const result = await axios.get<IOrderSettings>(
			`${this.API_URL}/order/settings`
		);
		return result.data;
	}

	public async addNewPaymentMethod(payment: IPayment): Promise<IPayment> {
		const result = await axios.post<IPayment>(
			`${this.API_URL}/order/settings/addNewPayment`,
			payment
		);
		return result.data;
	}

	public async addNewdeliveryMethod(delivery: IDelivery): Promise<IDelivery> {
		const result = await axios.post<IDelivery>(
			`${this.API_URL}/order/settings/addNewDelivery`,
			delivery
		);
		return result.data;
	}

	public async addNewDiscount(discount: IDiscount): Promise<IDiscount> {
		const result = await axios.post<IDiscount>(
			`${this.API_URL}/order/settings/addNewDiscount`,
			discount
		);
		return result.data;
	}
}
