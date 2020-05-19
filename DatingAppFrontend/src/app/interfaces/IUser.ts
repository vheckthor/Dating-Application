import { IPhoto } from './IPhoto';

export interface IUser {
    id: number;
    username: string;
    userUniqueIdentity: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    introduction: string;
    phoneNumber?: string;
    lookingFor: string;
    photos?: IPhoto[];
    personalityType: string;
}
