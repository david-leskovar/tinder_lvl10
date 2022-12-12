import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  NgForm,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { from } from 'rxjs';
import { AuthService, User, UserRegister } from '../Services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  registerForm: FormGroup;
  loading: boolean = false;

  initializeForm() {
    this.registerForm = new FormGroup({
      username: new FormControl('', [
        Validators.minLength(5),
        Validators.maxLength(15),
        Validators.required,
      ]),
      password: new FormControl('', [
        Validators.minLength(5),
        Validators.maxLength(15),
        Validators.required,
      ]),
      confirmPassword: new FormControl('', [
        Validators.minLength(5),
        Validators.maxLength(15),
        Validators.required,
        this.mathValues('password'),
      ]),
      gender: new FormControl('Male', Validators.required),
      city: new FormControl('', [
        Validators.minLength(3),
        Validators.maxLength(20),
        Validators.required,
      ]),
      knownAs: new FormControl('', [
        Validators.minLength(5),
        Validators.maxLength(15),
        Validators.required,
      ]),
      country: new FormControl('', [
        Validators.minLength(5),
        Validators.maxLength(15),
        Validators.required,
      ]),
      dateOfBirth: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    let user: UserRegister = {
      username: this.registerForm!.get('username')!.value,
      password: this.registerForm!.get('password')?.value,
      knownAs: this.registerForm.get('knownAs')?.value,
      gender: this.registerForm.get('gender')?.value,
      city: this.registerForm.get('city')?.value,
      country: this.registerForm.get('country')?.value,
      dateOfBirth: this.registerForm.get('dateOfBirth')?.value,
    };

    this.authService.register(user).subscribe(
      (data) => {
        this.toastr.success('Succesful register');
        this.router.navigate(['']);
      },
      (error) => {
        console.log(error);
      }
    );
  }

  mathValues(matchTo: any): ValidatorFn {
    return (control: any) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  ngOnInit(): void {
    this.initializeForm();
  }
}
