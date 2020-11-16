
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '.././auth.service';

@Component({
  selector: "login",
  templateUrl: "./login.component.html",
})

export class LoginComponent {

  form: FormGroup;

  constructor(
    public auth: AuthService,
    private fb: FormBuilder,
  ) {

    this.form = fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login(formValue: any) {
    this.auth.login(formValue);
  }
}

