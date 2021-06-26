import ICustomer from "@/models/request/customer/customer";
import axios from "axios";

/**
 * Customer Service
 */
export default class CustoemrService {
	API_URL = process.env.VUE_APP_API_URL;

	public async getCustomers(): Promise<ICustomer[]> {
		const result = await axios.get<ICustomer[]>(
			`${this.API_URL}/customer/`
		);
		return result.data;
	}

	public async addCustomer(newCustomer: ICustomer): Promise<ICustomer> {
		const result = await axios.post<ICustomer>(
			`${this.API_URL}/customer/`,
			newCustomer
		);
		return result.data;
	}

	public async refreshCustomer(
		customerid: number,
		refreshedCustomer: ICustomer
	): Promise<ICustomer> {
		const result = await axios.patch<ICustomer>(
			`${this.API_URL}/customer/${customerid}`,
			refreshedCustomer
		);
		return result.data;
	}

	public async deleteCustomer(customerid: number): Promise<boolean> {
		const result = await axios.delete<boolean>(
			`${this.API_URL}/customer/${customerid}`
		);
		return result.data;
	}
}
