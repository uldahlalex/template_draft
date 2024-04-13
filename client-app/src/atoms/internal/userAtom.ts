// state/atoms/effects.ts
import {atomWithStorage} from 'jotai/utils';

export interface UserAtom {
    username: string;
    id: number;
}

export const userAtom = atomWithStorage<UserAtom | null>('user', null);



