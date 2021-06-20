import ICustomer from "./customer";
import IDelivery from "./delivery";
import IDiscount from "./discount";
import IOrderItem from "./order-item";
import IPayment from "./payment";

export enum OrderStatus {
	Created,
	Shipped,
	Delivered,
	Declined,
	Cancelled,
	Completed,
}

export default interface IOrder {
	id: number;
	customer: ICustomer;
	items: IOrderItem[];
	orderStatus: OrderStatus;
	delivery: IDelivery;
	paument: IPayment;
	discount: IDiscount;
	additionalInfo: string;
	totalPrice: number;
	createdOn: Date;
}

export interface INewOrder {
	id: number;
	customerId: number;
	items: IOrderItem[];
	orderStatus: OrderStatus;
	deliveryId: number;
	paymentId: number;
	discountId: number;
	additionalInfo: string;
	totalPrice: number;
}
