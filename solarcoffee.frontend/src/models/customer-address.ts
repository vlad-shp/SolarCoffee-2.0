export default interface ICustomerAddress {
	id: number;

	createdOn: Date;
	updatedOn: Date;

	addressLine1: string;
	addressLine2: string;
	city: string;
	state: string;
	postalCode: string;
	country: string;
}
