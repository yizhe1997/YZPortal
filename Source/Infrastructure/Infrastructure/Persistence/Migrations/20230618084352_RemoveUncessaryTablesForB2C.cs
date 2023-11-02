using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUncessaryTablesForB2C : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipContentAccessLevels");

            migrationBuilder.DropTable(
                name: "MembershipDealerRoles");

            migrationBuilder.DropTable(
                name: "MembershipNotifications");

            migrationBuilder.DropTable(
                name: "UserInviteDealerSelections");

            migrationBuilder.DropTable(
                name: "UserPasswordResets");

            migrationBuilder.DropTable(
                name: "ContentAccessLevels");

            migrationBuilder.DropTable(
                name: "DealerRoles");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "UserInvites");

            migrationBuilder.DropTable(
                name: "Dealers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentAccessLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentAccessLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailedMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ValidUntilDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPasswordResets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CallbackUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaimedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailedMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ValidUntilDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPasswordResets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPasswordResets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInviteDealerSelections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UserContentAccessLevels = table.Column<int>(type: "int", nullable: false),
                    UserInviteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInviteDealerSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInviteDealerSelections_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInviteDealerSelections_UserInvites_UserInviteId",
                        column: x => x.UserInviteId,
                        principalTable: "UserInvites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MembershipContentAccessLevels",
                columns: table => new
                {
                    ContentAccessLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipContentAccessLevels", x => new { x.ContentAccessLevelId, x.MembershipId });
                    table.ForeignKey(
                        name: "FK_MembershipContentAccessLevels_ContentAccessLevels_ContentAccessLevelId",
                        column: x => x.ContentAccessLevelId,
                        principalTable: "ContentAccessLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipContentAccessLevels_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipDealerRoles",
                columns: table => new
                {
                    DealerRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipDealerRoles", x => new { x.DealerRoleId, x.MembershipId });
                    table.ForeignKey(
                        name: "FK_MembershipDealerRoles_DealerRoles_DealerRoleId",
                        column: x => x.DealerRoleId,
                        principalTable: "DealerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembershipDealerRoles_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailedMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptedSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipNotifications_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentAccessLevels_Name",
                table: "ContentAccessLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerRoles_Name",
                table: "DealerRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_Name",
                table: "Dealers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipContentAccessLevels_MembershipId",
                table: "MembershipContentAccessLevels",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipDealerRoles_MembershipId",
                table: "MembershipDealerRoles",
                column: "MembershipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipNotifications_MembershipId",
                table: "MembershipNotifications",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_DealerId",
                table: "Memberships",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInviteDealerSelections_DealerId",
                table: "UserInviteDealerSelections",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInviteDealerSelections_UserInviteId",
                table: "UserInviteDealerSelections",
                column: "UserInviteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswordResets_UserId",
                table: "UserPasswordResets",
                column: "UserId");
        }
    }
}
