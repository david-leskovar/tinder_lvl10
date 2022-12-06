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
import { AuthService } from '../Services/auth.service';

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
    });
  }

  onSubmit() {
    console.log(this.registerForm);
    /*this.loading = true;
    let username = form.value.username;
    let password = form.value.password;
    this.authService
      .register({
        username: form.value.username,
        password: form.value.password,
      })
      .subscribe(
        (data) => {
          console.log(data);
          this.authService
            .login({
              username: username,
              password: password,
            })
            .subscribe(() => {
              console.log('ka daj');

              this.loading = false;
              this.router.navigate(['']);
            });
        },
        (error) => {
          console.log(error);

          this.toastr.error('Hello bozo, there was an error!');
          this.loading = false;
        }
      );
      */
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
