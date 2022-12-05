export interface Photo {
  id: number;
  url: string;
  isMain: boolean;
}

export interface Member {
  id: string;
  username: string;
  photoURL: string;
  age: number;
  knownAs: string;
  created: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  photos: Photo[];
}
