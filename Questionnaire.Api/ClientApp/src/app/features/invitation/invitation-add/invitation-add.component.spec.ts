import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationAddComponent } from './invitation-add.component';

describe('InvitationAddComponent', () => {
  let component: InvitationAddComponent;
  let fixture: ComponentFixture<InvitationAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitationAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitationAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
