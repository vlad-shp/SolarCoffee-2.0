import ICustomerAddress from "./customer-address";

export default interface ICustomer {
	id: number;

	primaryAddress: ICustomerAddress;

	firstName: string;
	lastName: string;
	email: string;
	phoneNumber: string;

	createdOn: Date;
	updatedOn: Date;
}
