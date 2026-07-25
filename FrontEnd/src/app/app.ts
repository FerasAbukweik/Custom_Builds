import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { projectName } from '../core/constants/project';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal(projectName);
}
