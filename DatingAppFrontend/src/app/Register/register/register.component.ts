import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/allservices/auth.service';
import { AlertifyService } from 'src/app/allservices/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder, FormGroupName } from '@angular/forms';
import {countries} from 'countries-list';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { SearchCountryField, TooltipLabel, CountryISO } from 'ngx-intl-tel-input';
import { IRegister } from 'src/app/interfaces/IRegister';
import { Imodel } from 'src/app/interfaces/Imodel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  // countries code
  separateDialCode = true;
  SearchCountryField = SearchCountryField;
  TooltipLabel = TooltipLabel;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [CountryISO.Nigeria];

  model: IRegister;
  login: Imodel;
  registerationForm: FormGroup;
  countriesArray: string[];
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authservices: AuthService, private alertify: AlertifyService, 
              private router: Router, private formbuild: FormBuilder) {

  }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
    this.getCountryList();
  }

  createRegisterForm(){
    this.registerationForm = this.formbuild.group({
      gender: ['male'],
      username:['', Validators.required],
      knownAs: [ '', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: [ '', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]],
      confirmPassword: ['', Validators.required],
      phoneNumber: ['', [Validators.required]]
    }, {validators: [this.passwordMatchValidator, this.ageValidator]});
  }


  passwordMatchValidator(glove: FormGroup){
    return glove.get('password').value === glove.get('confirmPassword').value ? null : { mismatch : true };
  }

  ageValidator(game: FormGroup){
    const today = new Date();
    const birthDate = new Date(game.get('dateOfBirth').value);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age >= 18 ? null : {tooyoung: true};
  }

  getCountryList(){
    const countryCodes = Object.keys(countries);
    this.countriesArray = countryCodes.map(code => countries[code].name);
  }

  register(){
    if (this.registerationForm.valid){
      const moel = Object.assign({}, this.registerationForm.value);
      this.model = {...moel};
      this.model.phoneNumber = [moel.phoneNumber];
      this.login = {username: moel.username, password: moel.password};

      this.authservices.register(this.model).subscribe(next => {
        this.alertify.success('Registration Successful');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authservices.login(this.login).subscribe(() => {
          this.router.navigate(['/members']);
        }, error => {
          this.alertify.error(error);
        });
      });
    }
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
