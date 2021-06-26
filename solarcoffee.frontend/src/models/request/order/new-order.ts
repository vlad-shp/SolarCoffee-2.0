import IOrderItem from "./order-item";

export enum OrderStatus {
	Created,
	Shipped,
	Delivered,
	Declined,
	Cancelled,
	Completed,
}

export interface INewOrder {
	id: number;
	customerId: number;
	orderItems: IOrderItem[];
	orderStatus: OrderStatus;
	deliveryId: number;
	paymentId: number;
	discountId: number;
	additionalInfo: string;
	totalPrice: number;
}
