export interface User {
  userName?: string;
  token: string;
  knownAs: string;
  gender: string;
  photoUrl?: string;
  roles?: string[];
}
