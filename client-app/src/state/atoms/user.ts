// state/atoms/user.ts
import { atom } from 'jotai';
import { atomWithStorage } from 'jotai/utils';
import { atomEffect } from 'jotai-effect'
import { toast } from 'react-hot-toast';
import {decodeJwt} from "./decodeJwt.ts";
import {useNavigate} from "react-router-dom";

export interface User {
    username: string;
    id: number;
}

export const userAtom = atomWithStorage<User | null>('user', null);

export const sayHiWhenUserChanges = atomEffect((get, set) => {
    const user = get(userAtom);
    console.log(user)
    if (user) {
        toast.success(`Welcome back ${user.username}`);
    } else {
        console.log('No user');
    }
});

export const jwtAtom = atomWithStorage<string>('token', '');

export const decodeAndSetUserWhenJwtAtomChanges = atomEffect((get, set) => {
    const jwt = get(jwtAtom);
    console.log(jwt)
    if (jwt) {
        set(userAtom, decodeJwt(jwt));
    }
});
export const getBackToLoginWhenJwtIsRemoved = atomEffect((get, set) => {
    //This means sign out will essentially just be:
    // localStorage.removeItem('token');
    const jwt = get(jwtAtom);
    if (!jwt) {
        console.log("tsx comp now reponsible for this instead")
    }
});