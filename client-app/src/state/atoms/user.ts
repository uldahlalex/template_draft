// state/atoms/user.ts
import { atom } from 'jotai';
import { atomWithStorage } from 'jotai/utils';
import { atomEffect } from 'jotai-effect'
import { toast } from 'react-hot-toast';

export interface User {
    username: string;
    id: number;
}

export const userAtom = atomWithStorage<User | null>('user', null);

export const userEffect = atomEffect((get, set) => {
    const user = get(userAtom);
    console.log(user)
    if (user) {
        toast('Welcome back ' + user.username);
    } else {
        console.log('No user');
    }
});