import { OrderStatus } from "../request/order/new-order";
import ICustomer from "../request/customer/customer";
import IOrderItemView from "./order-item-view";
import IDelivery from "../request/order/delivery";
import IPayment from "../request/order/payment";
import IDiscount from "../request/order/discount";

export default interface IOrderView {
	id: number;
	customer: ICustomer;
	orderItems: IOrderItemView[];
	orderStatus: OrderStatus;
	delivery: IDelivery;
	payment: IPayment;
	discount: IDiscount;
	additionalInfo: string;
	totalPrice: number;
	createdOn: Date;
	updatedOn: Date;
}
