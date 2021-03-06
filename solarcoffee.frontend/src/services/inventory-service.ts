import IInventory from "@/models/request/inventory";
import { IShipment } from "@/models/request/shipment";
import axios from "axios";

export default class InventoryService {
	API_URL = process.env.VUE_APP_API_URL;
	public async getInventory(): Promise<IInventory[]> {
		const result = await axios.get<IInventory[]>(
			`${this.API_URL}/inventory/`
		);
		return result.data;
	}

	public async updateInventory(shipment: IShipment): Promise<IInventory> {
		const result = await axios.patch<IInventory>(
			`${this.API_URL}/inventory/`,
			shipment
		);
		return result.data;
	}

	public async getInventoryItemByProductId(
		productId: number
	): Promise<IInventory> {
		const result = await axios.get<IInventory>(
			`${this.API_URL}/inventory/${productId}`
		);
		return result.data;
	}
}
