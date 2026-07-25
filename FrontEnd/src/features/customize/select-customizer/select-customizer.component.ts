import { Component } from '@angular/core';
import { IOption } from './select-customizer.model';
import { RouterLink } from '@angular/router';
import { TopNavComponent } from '../../../layouts/top-nav/top-nav.component';

@Component({
  selector: 'app-select-customizer',
  imports: [TopNavComponent, RouterLink],
  templateUrl: './select-customizer.component.html',
})
export class SelectCustomizerComponent {
  options: IOption[] = [
    {
      title: 'Controller',
      subtitle: 'Precision Engineering',
      description:
        'Engineered for professional play with hall-effect triggers and remappable back paddles.',
      imgSrc: 'assets/images/keyboard-image.png',
      goTo: 'controller',
    },
    {
      title: 'Keyboard',
      subtitle: 'Tactile Mastery',
      description:
        'Hot-swappable switches, sound-damped gaskets, and aircraft-grade aluminum casing.',
      imgSrc: 'assets/images/keyboard-image.png',
      goTo: 'keyboard',
    },
  ];
}
