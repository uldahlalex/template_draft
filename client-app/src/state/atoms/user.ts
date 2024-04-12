// state/atoms/user.ts
import { atom } from 'jotai';
import { atomWithStorage } from 'jotai/utils';
import { atomEffect } from 'jotai-effect'
import { toast } from 'react-hot-toast';
import {decodeJwt} from "./decodeJwt.ts";

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