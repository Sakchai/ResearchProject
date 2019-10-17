using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Core.Domain;
using Research.Web.Models.Users;
using FluentValidation;
using System;

namespace Research.Web.Validators.Users
{

    public partial class RegisterValidator : BaseResearchValidator<RegisterModel>
    {
        public RegisterValidator(IUserService userService,
            IDbContext dbContext)
        {
            RuleFor(x => x.TitleId)
                .NotEqual(0)
                .WithMessage("ระบุคำนำหน้าชื่อ!");

            RuleFor(x => x.AgencyId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสหน่วยงาน!");

            //ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("ระบุอีเมล!");


            RuleFor(x => x.Email).Must((x, context) =>
            {
                return IsDuplicateEmailChecked(x, userService);
            }).WithMessage("อีเมลซ้ำในระบบ ระบุอีเมลใหม่อีกครั้ง!");


            //form fields
            RuleFor(x => x.IDCard)
                .NotEmpty()
                .WithMessage("ระบุเลขประจำตัวประชาชน!");

            RuleFor(x => x.IDCard)
                .Length(13)
                .WithMessage("ระบุเลขประจำตัวประชาชนจำนวน 13 หลัก!");

            RuleFor(x => x.IDCard)
                .NotEmpty()
                .Matches(@"^\d{13}$")
                .WithMessage("ระบุเลขประจำตัวประชาชนเป็นตัวเลขเท่านั้น!");

            RuleFor(x => x.IDCard)
                .NotEmpty()
                .Must((x, context) =>
            {
                return IsDuplicateIDCardChecked(x, userService);
            }).WithMessage("เลขประจำตัวประชาชนซ้ำในระบบ กรุณาระบุเลขประจำตัวประชาชนใหม่อีกครั้ง!");

            //RuleFor(x => x.IDCard)
            //    .NotEmpty()
            //    .Must((x, context) =>
            //{
            //    return IsValidIDCard(x);
            //}).WithMessage("เลขประจำตัวประชาชนไม่ถูกต้อง กรุณาระบุเลขประจำตัวประชาชนใหม่อีกครั้ง!");


            SetDatabaseValidationRules<User>(dbContext);
        }


        private bool IsDuplicateEmailChecked(RegisterModel model, IUserService userService)
        {
            var user = userService.GetUserByEmail(model.Email);
            return user != null ? false : true;
        }

        private bool IsDuplicateIDCardChecked(RegisterModel model, IUserService userService)
        {
            var user = userService.GetUserByIDCard(model.IDCard);
            return user != null ? false : true;
        }

        private bool IsValidIDCard(RegisterModel model)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                string pos = ((char)model.IDCard[i]).ToString();
                sum += int.Parse(pos) * (13 - i);
            }
            string pos12 = ((char) model.IDCard[12]).ToString();

            return ((11 - sum % 11) % 10 == int.Parse(pos12));

        }

        private bool IsValidIDCard2(RegisterModel model)
        {
            long id = long.Parse(model.IDCard);
            long _base = 100000000000l; //สร้างตัวแปร เพื่อสำหรับให้หารเพื่อเอาหลักที่ต้องการ
            int basenow; //สร้างตัวแปรเพื่อเก็บค่าประจำหลัก
            int sum = 0; //สร้างตัวแปรเริ่มตัวผลบวกให้เท่ากับ 0
            for (int i = 13; i > 1; i--)
            { //วนรอบตั้งแต่ 13 ลงมาจนถึง 2
                basenow = (int)Math.Floor((decimal)id / _base); //หาค่าประจำตำแหน่งนั้น ๆ
                id = id - basenow * _base; //ลดค่า id ลงทีละหลัก
                Console.WriteLine(basenow + "x" + i + " = " + (basenow * i)); //แสดงค่าเมื่อคูณแล้วของแต่ละหลัก
                sum += basenow * i; //บวกค่า sum ไปเรื่อย ๆ ทีละหลัก
                _base = _base / 10; //ตัดค่าที่ใช้สำหรับการหาเลขแต่ละหลัก
            }
            Console.WriteLine("Sum is " + sum); //แสดงค่า sum
            int checkbit = (11 - (sum % 11)) % 10; //คำนวณค่า checkbit
            Console.WriteLine("Check bit is " + checkbit); //แสดงค่า checkbit 

            return checkbit == id;
        }
    }
}