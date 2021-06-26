import IProduct from "../request/product";

export default interface IOrderItemView {
	id: number;
	product: IProduct;
	quantity: number;
}
