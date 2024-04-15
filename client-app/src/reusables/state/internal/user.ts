import {atomWithStorage} from 'jotai/utils';

export interface User {
    username: string;
    id: number;
}

export const userAtom = atomWithStorage<User | null>('user', null);




