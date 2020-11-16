
import { Component } from '@angular/core';
import { ApiService } from '.././api.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '.././auth.service';

@Component({
  selector: "register",
  templateUrl: "./register.component.html",
})

export class RegisterComponent {

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

  register(credentials: any) {
    this.auth.register(credentials);
  }

}

