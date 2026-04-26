import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { IMessage } from './support.model';

@Component({
  selector: 'app-support',
  imports: [CommonModule],
  templateUrl: './support.component.html',
  host: {
    class:
      'flex flex-col h-full bg-dark-deep-blue rounded-xl overflow-hidden border border-dark-blue-gray shadow-2xl max-w-450 mx-auto',
  },
})
export class SupportComponent {
  messages: IMessage[] = [
    {
      id: 1,
      sender: 'agent',
      name: 'Marcus',
      role: 'Technical Specialist',
      content:
        "Hi there! I'm Marcus from the tech team. I see you're having some trouble with your Neo-Tokyo build's LED firmware. How can I help?",
      time: '14:18',
      type: 'text',
    },
    {
      id: 2,
      sender: 'user',
      name: 'malek',
      content:
        "Yeah, the spacebar RGB isn't syncing with the rest of the board after the last update.",
      time: '14:19',
      type: 'text',
    },
    {
      id: 3,
      sender: 'agent',
      name: 'Marcus',
      content:
        'Understood. Please try flashing this patch. It addresses the sub-controller sync issues on Neo-Tokyo PCBs.',
      time: '14:20',
      type: 'file',
      fileName: 'firmware_fix_v2.bin',
    },
  ];

  quickActions = [
    'Where is my order?',
    'Customization help',
    'Payment issue',
    'Firmware update',
    'Warranty claim',
  ];
}
