import { User } from '../Services/auth.service';

export class UserParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';
  constructor(user: User) {
    this.gender = user.gender?.toLowerCase() === 'female' ? 'male' : 'female';
  }
}
