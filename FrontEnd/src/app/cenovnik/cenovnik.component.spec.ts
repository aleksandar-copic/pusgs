import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CenovnikComponent } from './cenovnik.component';

describe('CenovnikComponent', () => {
  let component: CenovnikComponent;
  let fixture: ComponentFixture<CenovnikComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CenovnikComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CenovnikComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
