import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionnaireResultAdminComponent } from './questionnaire-result-admin.component';

describe('QuestionnaireResultAdminComponent', () => {
  let component: QuestionnaireResultAdminComponent;
  let fixture: ComponentFixture<QuestionnaireResultAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionnaireResultAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionnaireResultAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
