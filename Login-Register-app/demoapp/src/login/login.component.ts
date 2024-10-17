import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'; // Import Router for navigation

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  showPassword: boolean = false;
  loginError: string = ''; // For displaying error messages

  constructor(private fb: FormBuilder, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword; // Toggle password visibility
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { username, password, rememberMe } = this.loginForm.value;

      // Placeholder for authentication logic
      // Replace this with actual authentication service
      if (username === 'testuser' && password === 'password123') {
        console.log('Login successful');
        // Redirect to a different page upon successful login
        this.router.navigate(['/dashboard']); // Navigate to the dashboard
      } else {
        this.loginError = 'Invalid username or password'; // Set error message
      }
    } else {
      this.loginError = 'Please fill in all required fields'; // Error for empty fields
    }
  }
}
