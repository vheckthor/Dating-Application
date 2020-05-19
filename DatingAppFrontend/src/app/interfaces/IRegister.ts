import { IPhonenumber } from './IPhonenumber';

export interface IRegister{
    gender: string;
    username: string;
    knownAs: string;
    dateOfBirth: string;
    city: string;
    country: string;
    password: string;
    phoneNumber: IPhonenumber[];
}