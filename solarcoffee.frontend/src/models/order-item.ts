import IProduct from "./product";

export default interface IOrderItem {
	id: number;
	productId: number;
	productName: string;
	productPrice: number;
	quantity: number;
}
