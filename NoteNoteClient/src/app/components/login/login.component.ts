import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms'; 
import { CardModule } from 'primeng/card';
import { FloatLabelModule } from 'primeng/floatlabel';
import { UserApiService } from '../../services/user/user-api.service';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-login',
  imports: [CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule,
    FloatLabelModule,
    SelectButtonModule,
    ToastModule
  ],
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [MessageService]
})
export class LoginComponent {
  stateOptions: any[] = [{ label: 'Login', value: 'Login' },{ label: 'Register', value: 'Register' }];

  value: string = 'Login';

  username: string = '';
  password: string = '';
  email: string = '';
  errorMessage: string = '';

  constructor(private apiService: UserApiService, private router: Router, private messageService: MessageService) { }

  login(): void {
    this.apiService.login(this.username, this.password).subscribe(
      (response) => {
        console.log('Login successful:', response);
        
        localStorage.setItem('userId', response.userId.toString());
        localStorage.setItem('token', response.token.toString());
        
        this.router.navigateByUrl('/').then(()=>{
          location.reload();
        });
      },
      (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.error.message });
      }
    );
  }

  register(): void {
    this.apiService.register(this.username, this.password, this.email).subscribe(
      (response) => {
        console.log('Register successful:', response);
        
        localStorage.setItem('userId', response.userId.toString());
        
        this.router.navigateByUrl('/').then(()=>{
          location.reload();
        });
      },
      (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.error.message });
      }
    );
  }
}
