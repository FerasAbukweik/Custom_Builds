import { Component } from '@angular/core';
import { LiveChattingComponent } from "../../../../../shared/components/live-chatting/live-chatting.component";

@Component({
  selector: 'app-support-chat.component',
  imports: [LiveChattingComponent],
  templateUrl: './support-chat.component.html',
})
export class SupportChatComponent {
  navItems = [
    { icon: 'dashboard', name: 'Dashboard', active: false },
    { icon: 'shopping_bag', name: 'Orders', active: false },
    { icon: 'inventory_2', name: 'Inventory', active: false },
    { icon: 'group', name: 'Customers', active: false },
    { icon: 'palette', name: 'Design Library', active: false },
    { icon: 'forum', name: 'Support Chat', active: true }
  ];

  activeChats = [
    { 
      name: 'Alex R.', 
      time: '14:22', 
      snippet: 'Typing...', 
      active: true, 
      online: true, 
      isTyping: true, 
      img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAFbAz4Og8kqbKjL0ZX_tB0TUj4dVJoB2YLf47YwKc2M0h7Pue_WgsTt2iM4NqbewSAT_0LPGzndx2YaWVfj8iKAPyPuXMlBXE-dK1fKAR0NYomc7ODUWnvFopx6_owWYwFy5c58S2cPvky-_hI-z4--pqjUtyajAtEnxfGiNKUYdlUY-wTbSomx35S5bMciMx40m20D9bMTbneFQUL8c56jA5piCirwG3emaYf4w7ZzPAqbC9nS3Tq0np4ZBGZ0l_SrbPxKe5sNQU_' 
    },
    { 
      name: 'Sarah C.', 
      time: '12:05', 
      snippet: 'The inventory sync is complete.', 
      active: false, 
      online: true, 
      isTyping: false, 
      img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuCJx7inObVLkW801z2MOfbOHeQi2NjkEtrnMz6Y5P0OqaZdE5OD8r9f09KJv2hErsjeHc4YYWLH5Xsz4dWLx8HWAPlWKIvVwVDsTGnDjiyHbunCZTcr-IdhXoGAKiInFQvtAwVIuR1VESvvpdJgBHy-8OM2DwCVjaL3lAZulCDXDUBLTGIVdkFgcoTHu_4iJvZ_ilIug2tmJx9G5uheEEZH3Km1BZaoYrf0abtDaOT43fLEyrDYJy2ie5c_qq2Oo4ZohiX1UXcFFfQO' 
    },
    { 
      name: 'Marcus T.', 
      time: 'Yesterday', 
      snippet: 'Thanks for the update on the shipment.', 
      active: false, 
      online: false, 
      isTyping: false, 
      img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDPDQArhlPY3O_5z-N0SC1Ka4RqKokIRq3foZhvAIA4ZjBm27jED6UNnEsw34q_SpfTnRxlVAnEoDxYng1qyaEoChWedN57zQ4wn4ZjtodB262gT5X52sOGwEmM8NKb4TnXMK3ruBiSz2gdjP3uHaVw35H04_Rga7iKtHbov7NUYI25ZDIeJWI31I_tc8JUKsCo1PFjcCOBhSHa3iEBerlOSM-StMS5UN0oBxrcjcGYeueo9aQIyfgx9RDVq8aTxLE555CEV9-375zC' 
    }
  ];

  chatMessages = [
    { 
      type: 'received', 
      text: "Hi there! I'm having some trouble with the inventory sync for the new core modules. It seems like the API is returning a 403 error on the latest endpoint.", 
      time: '14:15', 
      img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAFbAz4Og8kqbKjL0ZX_tB0TUj4dVJoB2YLf47YwKc2M0h7Pue_WgsTt2iM4NqbewSAT_0LPGzndx2YaWVfj8iKAPyPuXMlBXE-dK1fKAR0NYomc7ODUWnvFopx6_owWYwFy5c58S2cPvky-_hI-z4--pqjUtyajAtEnxfGiNKUYdlUY-wTbSomx35S5bMciMx40m20D9bMTbneFQUL8c56jA5piCirwG3emaYf4w7ZzPAqbC9nS3Tq0np4ZBGZ0l_SrbPxKe5sNQU_' 
    },
    { 
      type: 'sent', 
      text: "I've checked the logs. It looks like your access token for the Core-API expired earlier this morning. I'm regenerating a new scope for your account now.", 
      time: '14:18', 
      statusIcon: 'done_all' 
    },
    { 
      type: 'received', 
      text: "Perfect, thanks for the quick response. Will I need to restart the local peripheral nodes once the token is updated?", 
      time: '14:20', 
      img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAiUR89nPkxuFqLu4d63sezM-uZ7BnaLZDYqACt6Ho8gga7fBNU0Sv-ee2YPG5_Xolh4i10MB2r1KSj9-Qbu1qkMDZzpALmXMAdfRUBcpSdFZq96SViusHMoowA-geQ0seKUpjlId_vuR7y2nD41N5oXizwXv_QzG9rSNVZUoN27nx1h3y8iw4o8nW-swarM8OPRwGiRPWDztk7NoV4QUMhEvr5wLqJ3EXyg1rR6MAgbUfoLPOWSOWgbo-J0ucyPaubgqfIUWqs7gcF' 
    }
  ];

  sharedAssets = [
    { name: 'Inventory_Report_Q4.pdf', size: '2.4 MB', date: 'Oct 22', icon: 'description' },
    { name: 'API_Specs_V2.json', size: '156 KB', date: 'Oct 21', icon: 'receipt_long' }
  ];
}
